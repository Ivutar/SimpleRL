using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M4
{
    public static class RayTracer
    {        

        public static Intersect Ray (double x, double y, double angle, int cellsize, Func<int, int, int> get_cell)
        {
            ////////////////////////
            // check input values //
            ////////////////////////

            //min size for cells (i.e. ONE cell contains ONE column by default)
            cellsize = cellsize < 1 ? 1 : cellsize;

            //always filled cell by default
            if (get_cell == null)
                get_cell = (cellx, celly) => 1;

            //////////////////
            // calcualtions //
            //////////////////

            //ray endpoints
            double x1 = x;
            double y1 = y;
            double x2 = x + Math.Cos(angle);
            double y2 = y + Math.Sin(angle);

            //current cell (zero-based index)
            int cx = (int)Math.Floor(x / cellsize);
            int cy = (int)Math.Floor(y / cellsize);

            //increments for cell
            int icx = x2 - x1 > 0 ? 1 : -1;
            int icy = y2 - y1 > 0 ? 1 : -1;

            //Ax+By+C=0
            double A = y1 - y2;
            double B = x2 - x1;
            double C = -A * x1 - B * y1;

            //x=V, y=H
            double V = x2 > x1 ? (cx + 1) * cellsize : cx * cellsize;
            double H = y2 > y1 ? (cy + 1) * cellsize : cy * cellsize;

            //first vertical intersect
            //{x=V
            //{Ax+By+C=0 -> AV+By+C=0 -> y=(-C-AV)/B
            double vx = V;
            double vy = B != 0 ? -(C + A* V) / B : icy * 1000000 * cellsize; //avoid division by zero
            double vl = Math.Sqrt((vx - x1) * (vx - x1) + (vy - y1) * (vy - y1)); //length (x1,y1) - (vx,vy)

            //first horizontal intersect
            //{y=H
            //{Ax+By+C=0 -> Ax+BH+C=0 -> x = (-C-BH)/A
            double hy = H;
            double hx = A != 0 ? -(C + B * H) / A : icx * 1000000 * cellsize; //avoid division by zero
            double hl = Math.Sqrt((hx - x1) * (hx - x1) + (hy - y1) * (hy - y1)); //length (x1,y1) - (hx,hy)

            //vertical intersection increments
            double ivx = cellsize * icx;
            double ivy = x2 != x1 ? cellsize * (y2 - y1) / (x2 - x1) : icy * 1000000 * cellsize; //avoid division by zero
            double ivl = Math.Sqrt(ivx * ivx + ivy * ivy);

            //horizontal intersection increments
            double ihx = y2 != y1 ? cellsize * (x2 - x1) / (y2 - y1) : icx * 1000000 * cellsize; //avoid division by zero
            double ihy = cellsize * icy;
            double ihl = Math.Sqrt(ihx * ihx + ihy * ihy);

            /////////////
            // tracing //
            /////////////

            int c;
            Intersect intersect = new Intersect();

            int i = 0;
            while (++i < 999999) //avoid endless loop
                if (vl < hl)
                {
                    //check vertical intersection
                    if ((c = get_cell(cx + icx, cy)) > 0)
                    {
                        intersect.cell = c;
                        intersect.cellx = cx + icx;
                        intersect.celly = cy;
                        intersect.column = (int)Math.Truncate(Math.IEEERemainder(vy, cellsize));
                        intersect.distance = vl;
                        intersect.horizontal = false;
                        break;
                    }

                    //do increment
                    cx += icx;
                    vx += ivx;
                    vy += ivy;
                    vl += ivl;
                }
                else
                {
                    //check horizontal intersection
                    if ((c = get_cell(cx, cy + icy)) > 0)
                    {
                        intersect.cell = c;
                        intersect.cellx = cx;
                        intersect.celly = cy + icy;
                        intersect.column = (int)Math.Truncate(Math.IEEERemainder(hx, cellsize));
                        intersect.distance = hl;
                        intersect.horizontal = true;
                        break;
                    }

                    //do increment
                    cy += icy;
                    hx += ihx;
                    hy += ihy;
                    hl += ihl;
                }

            return intersect;
        }
    }

    public struct Intersect
    {
        public int cell;
        public int cellx;
        public int celly;
        public int column;
        public double distance;
        public bool horizontal;
    }
}
