using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M4
{
    public static class RayTracer
    {
        public struct Intersect
        {
            public int cellx;
            public int celly;
            public int column;
            public bool horizontal;
        }

        public static Intersect Trace(double x, double y, double angle, int cellsize, Func<int, int, bool> filled_cell, double dist = 1000)
        {
            //min view dist
            dist = dist < 1 ? 1 : dist;

            //min size for cells (i.e. ONE cell contains ONE column by default)
            cellsize = cellsize < 1 ? 1 : cellsize;

            //always filled cell by default
            if (filled_cell == null)
                filled_cell = (cellx, celly) => true;

            //end point
            double ex = x + dist * Math.Cos(angle);
            double ey = y + dist * Math.Sin(angle);

            //cell increment
            int stepX = ex - x > 0 ? 1 : -1;
            int stepY = ey - y > 0 ? 1 : -1;

            //first intersect
            double tMaxX = 0;
            double tMaxY = 0;

            //delta
            double tDeltaX = (ex - x) / (ey - y);
            double tDeltaY = (ey - y) / (ex - x);

            //traversal algorithm
            int cx = (int)Math.Floor(x);
            int cy = (int)Math.Floor(y);
            Intersect intersect;
            do
            {
                if (tMaxX < tMaxY)
                {
                    intersect.cellx = cx;
                    intersect.celly = cy;
                    intersect.horizontal = true;
                    intersect.column = (int)(tMaxX = Math.Truncate(tMaxX));

                    tMaxX = tMaxX + tDeltaX;
                    cx = cx + stepX;
                }
                else
                {
                    intersect.cellx = cx;
                    intersect.celly = cy;
                    intersect.horizontal = false;
                    intersect.column = (int)(tMaxY = Math.Truncate(tMaxY));

                    tMaxY = tMaxY + tDeltaY;
                    cy = cy + stepY;
                }
            } while (!filled_cell(cx, cy));

            return intersect;
        }
    }
}
