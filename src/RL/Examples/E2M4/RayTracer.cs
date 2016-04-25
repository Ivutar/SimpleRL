using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M4
{
    public static class RayTracer
    {        

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
            Intersect intersect = new Intersect();
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

        public static Intersect Ray (double x, double y, double angle, int cellsize, Func<int, int, int> get_cell, double dist = 1000)
        {
            ////////////////////////
            // check input values //
            ////////////////////////

            //min view dist
            dist = dist < 1 ? 1 : dist;

            //min size for cells (i.e. ONE cell contains ONE column by default)
            cellsize = cellsize < 1 ? 1 : cellsize;

            //always filled cell by default
            if (get_cell == null)
                get_cell = (cellx, celly) => 1;

            //////////////////
            // calcualtions //
            //////////////////

            //pre calc
            double cosa = Math.Cos(angle);
            double sina = Math.Sin(angle);

            //ray endpoints
            double x1 = x;
            double y1 = y;
            double x2 = x + dist * cosa;
            double y2 = y + dist * sina;

            //cell
            int cx = (int)Math.Ceiling(x / cellsize);
            int cy = (int)Math.Ceiling(y / cellsize);

            //cell increment
            int cix = x2 - x1 > 0 ? 1 : -1;
            int ciy = y2 - y1 > 0 ? 1 : -1;


            //...

            /////////////
            // tracing //
            /////////////

            //run ray trace
            Intersect intersect = new Intersect();
            //...
            return intersect;
        }
    }

    public struct Intersect
    {
        public int cell;
        public int cellx;
        public int celly;
        public int column;
        public bool horizontal;
    }
}
