using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;
using RL.FOV;

namespace E2M5
{
    class Maze
    {
        IFOV fov;
        int posx = 1;
        int posy = 1;

        int width { get { return map.GetLength(0); } }
        int height { get { return map.GetLength(1); } }

        int[,] map = new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,0,1,1,0,1,0,1,1,1,1,1,1,0,0,1},
            {1,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1},
            {1,0,1,0,0,1,1,0,1,1,1,1,1,0,0,0,0,1},
            {1,0,1,1,0,1,0,0,0,0,0,0,1,1,1,1,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
            {1,0,1,0,0,1,0,0,1,0,0,0,1,0,1,1,1,1},
            {1,0,1,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
            {1,0,1,0,0,1,0,0,0,0,0,0,1,1,1,0,1,1},
            {1,0,1,0,0,1,1,1,1,1,1,0,1,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
            {1,1,1,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
            {1,0,1,0,0,0,0,1,1,1,1,0,0,0,1,0,0,1},
            {1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        //0-hidden
        //1-explored
        //2-visibly
        int[,] visibility = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };

        public void Draw(int offsetx, int offsety, Canvas cvs)
        {
            cvs.Clear();
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (visibility[x,y] == 0)
                        cvs.Write(offsetx + x, offsety + y, "▒", Color.DarkGray, Color.Black);
                    else
                    {
                        Color fore = Color.LightGray;
                        Color back = Color.DarkGray;
                        if (visibility[x, y] == 2)
                        {
                            fore = Color.White;
                            back = Color.Black;
                        }

                        if (map[x, y] == 0)
                            cvs.Write(offsetx + x, offsety + y, " ", fore, back);
                        if (map[x, y] == 1)
                            cvs.Write(offsetx + x, offsety + y, "#", fore, back);
                    }
                }
            cvs.Write(offsetx + posx, offsety + posy, "@", Color.Yellow, Color.Black);
        }

        public void Move (int dx, int dy)
        {
            //remove visibility
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (visibility[x, y] == 2)
                        visibility[x, y] = 1;

            //move
            int newx = posx + dx;
            int newy = posy + dy;
            if (newx >= 0 && newy >= 0 && newx < width && newy < height)
                if (map[newx, newy] == 0)
                {
                    posx = newx;
                    posy = newy;
                }

            //update visibility
            fov.ComputeVisibility(posx, posy);
        }

        public Maze ()
        {
            fov = new RecursiveShadowCasting();
            fov.ViewRadius = 8;

            fov.IsSolidBlock = (x, y) =>
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return true;
                return map[x, y] != 0;
            };

            fov.SetBlockLightDistanceSquared = (x, y, dist2) =>
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return;
                visibility[x, y] = 2;
            };

            //update visibility
            fov.ComputeVisibility(posx, posy);
        }
    }
}
