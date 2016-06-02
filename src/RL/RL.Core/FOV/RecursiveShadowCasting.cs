using System;
//source: http://fadden.com/tech/ShadowCast.cs.txt

namespace RL.FOV
{
    public class RecursiveShadowCasting : IFOV
    {
        double viewRadius = 1;
        double viewRadiusSq = 1;
        int viewCeiling = 1;

        public double ViewRadius
        {
            get
            {
                return viewRadius;
            }
            set
            {
                viewRadius = value < 1 ? 1 : value;
                viewRadiusSq = viewRadius * viewRadius;
                viewCeiling = (int)Math.Ceiling(ViewRadius);
            }
        }

        public Func<int, int, bool> IsSolidBlock { get; set; }

        public Action<int, int, double> SetBlockLightDistanceSquared { get; set; }

        public void ComputeVisibility(int posx, int posy)
        {
            SetBlockLightDistanceSquared(posx, posy, 0.0f);

            for (int txidx = 0; txidx < s_octantTransform.Length; txidx++)
                CastLight(posx, posy, 1, 1.0f, 0.0f, s_octantTransform[txidx]);
        }

        #region [ tools ]

        private class OctantTransform
        {
            public int xx { get; private set; }
            public int xy { get; private set; }
            public int yx { get; private set; }
            public int yy { get; private set; }

            public OctantTransform(int xx, int xy, int yx, int yy)
            {
                this.xx = xx;
                this.xy = xy;
                this.yx = yx;
                this.yy = yy;
            }

            public override string ToString()
            {
                // consider formatting in constructor to reduce garbage
                return string.Format("[OctantTransform {0,2:D} {1,2:D} {2,2:D} {3,2:D}]",
                    xx, xy, yx, yy);
            }
        }

        private static OctantTransform[] s_octantTransform =
        {
            new OctantTransform( 1,  0,  0,  1 ),   // 0 E-NE
            new OctantTransform( 0,  1,  1,  0 ),   // 1 NE-N
            new OctantTransform( 0, -1,  1,  0 ),   // 2 N-NW
            new OctantTransform(-1,  0,  0,  1 ),   // 3 NW-W
            new OctantTransform(-1,  0,  0, -1 ),   // 4 W-SW
            new OctantTransform( 0, -1, -1,  0 ),   // 5 SW-S
            new OctantTransform( 0,  1, -1,  0 ),   // 6 S-SE
            new OctantTransform( 1,  0,  0, -1 ),   // 7 SE-E
        };

         void CastLight(
            int posx, int posy,
            int startColumn,
            double leftViewSlope, double rightViewSlope,
            OctantTransform txfrm)
        {
            bool prevWasBlocked = false;
            double savedRightSlope = -1;

            for (int currentCol = startColumn; currentCol <= viewCeiling; currentCol++)
            {
                int xc = currentCol;

                for (int yc = currentCol; yc >= 0; yc--)
                {
                    int gridX = posx + xc * txfrm.xx + yc * txfrm.xy;
                    int gridY = posy + xc * txfrm.yx + yc * txfrm.yy;

                    double leftBlockSlope = (yc + 0.5) / (xc - 0.5);
                    double rightBlockSlope = (yc - 0.5) / (xc + 0.5);

                    if (rightBlockSlope > leftViewSlope)
                        continue;
                    else if (leftBlockSlope < rightViewSlope)
                        break;

                    double distanceSquared = xc * xc + yc * yc;
                    if (distanceSquared <= viewRadiusSq)
                        SetBlockLightDistanceSquared(gridX, gridY, distanceSquared);

                    bool curBlocked = IsSolidBlock(gridX, gridY);

                    if (prevWasBlocked)
                    {
                        if (curBlocked)
                            savedRightSlope = rightBlockSlope;
                        else
                        {
                            prevWasBlocked = false;
                            leftViewSlope = savedRightSlope;
                        }
                    }
                    else
                    {
                        if (curBlocked)
                        {
                            if (leftBlockSlope <= leftViewSlope)
                                CastLight(posx, posy, currentCol + 1, leftViewSlope, leftBlockSlope, txfrm);

                            prevWasBlocked = true;
                            savedRightSlope = rightBlockSlope;
                        }
                    }
                }

                if (prevWasBlocked)
                    break;
            }
        }

        #endregion
    }
}