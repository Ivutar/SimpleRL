using RL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M4
{
    class Program
    {
        const double FOV = 75.0 / 180 * Math.PI; //Field Of View

        const double DELTA_ANGLE = 15.0 / 180 * Math.PI; //rotation speed (15 degrees in radians)
        const double DELTA_STEP = 16; //walk speed
        const int CELL_SIZE = 64;
        const int WALL_HEIGHT = 2 * CELL_SIZE;
        const int PLAYER_HEIGHT = WALL_HEIGHT * 2 / 3;
        const int SCREEN_DIST = CELL_SIZE / 3;
        const int SCREEN_HEIGHT = CELL_SIZE;

        static double player_x = CELL_SIZE * 1.5;
        static double player_y = CELL_SIZE * 1.5;
        static double player_angle = 15.0 / 180 * Math.PI;

        static int[,] map = new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,2,0,0,0,0,0,1,0,1,1,0,0,1},
            {1,0,2,0,1,0,1,0,0,0,0,0,0,1,1},
            {1,0,0,0,1,0,1,0,2,2,0,1,1,1,1},
            {1,3,0,1,1,0,0,0,0,2,0,0,0,0,1},
            {1,0,0,0,1,0,2,2,2,2,2,2,0,1,1},
            {1,1,0,1,1,0,0,0,0,0,2,2,0,1,1},
            {1,0,0,0,0,0,3,0,1,0,2,0,0,0,1},
            {1,0,3,3,3,0,3,0,1,0,2,0,1,0,1},
            {1,0,3,0,3,3,3,0,1,0,2,0,1,0,1},
            {1,0,0,0,0,0,3,0,1,0,0,0,1,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        static int MapHeight
        {
            get { return map.GetLength(0); }
        }

        static int MapWidth
        {
            get { return map.GetLength(1); }
        }

        static void Draw()
        {
            Util.Buffer.Clear();

            double current_angle = player_angle - FOV / 2;
            double fisheye_angle = -FOV / 2;
            double angle_step = FOV / Util.Buffer.Width;

            for (int x=0; x<Util.Buffer.Width; x++, current_angle += angle_step, fisheye_angle += angle_step)
            {
                Intersect pt = RayTracer.Ray
                (
                    player_x, player_y, current_angle, CELL_SIZE, 
                    (int cx, int cy) =>
                    {
                        if (cx < 0 || cy < 0 || cx >= MapWidth || cy >= MapHeight)
                            return 1; //outside indexes returns "wall"-cell
                        return map[cx, cy];
                    }
                );

                //choose fill char
                CharInfo ci = new CharInfo('▒', Color.White, Color.Black);
                if (pt.horizontal) ci.ch = '░';
                if (pt.cell == 1) ci.fore = Color.LightRed;
                if (pt.cell == 2) ci.fore = Color.LightGreen;
                if (pt.cell == 3) ci.fore = Color.LightBlue;

                //calc column height
                double dist = pt.distance * Math.Cos(fisheye_angle) + 0.1; //fisheye and zero dist correction 

                double a = SCREEN_DIST * (WALL_HEIGHT - PLAYER_HEIGHT) / dist;
                double b = SCREEN_DIST * PLAYER_HEIGHT / dist;

                int wd2 = Util.Buffer.Height / 2;
                int y1 = (int)(wd2 - (1.0 * a / SCREEN_HEIGHT * Util.Buffer.Height));
                int y2 = (int)(wd2 + 1.0 * b / SCREEN_HEIGHT * Util.Buffer.Height);

                //draw column
                for (int y = y1; y <= y2; y++)
                    Util.Buffer[x, y] = ci;
            }

            Util.Swap();
        }

        static void Walk(double step)
        {
            double nx = player_x + Math.Cos(player_angle) * step;
            double ny = player_y + Math.Sin(player_angle) * step;

            int cx = (int)(nx / CELL_SIZE);
            int cy = (int)(ny / CELL_SIZE);

            if (cx >= 0 && cy >= 0 && cx < MapWidth && cy < MapHeight)
                if (map[cx,cy] == 0)
                {
                    player_x = nx;
                    player_y = ny;
                }
        }

        static void Rotate(double da)
        {
            player_angle += da;
            if (player_angle < 0)            player_angle += 2 * Math.PI;
            if (player_angle >= 2 * Math.PI) player_angle -= 2 * Math.PI;
        }

        static void Main(string[] args)
        {
            Util.Title = "E2M4 - 3D Maze";
            Util.Width = 80;
            Util.Height = 24;
            Util.CursorVisible = false;

            while (true)
            {
                Draw();
                Util.Sleep(100);

                Event e = Events.GetNext(true); //pause on this line if event queue is empty
                if (e.Kind == EventKind.Key && e.Key.Press)
                {
                    if (e.Key.Key == ConsoleKey.Escape) break;
                    if (e.Key.Key == ConsoleKey.LeftArrow ) Rotate (-DELTA_ANGLE);
                    if (e.Key.Key == ConsoleKey.RightArrow) Rotate (+DELTA_ANGLE);
                    if (e.Key.Key == ConsoleKey.UpArrow   ) Walk   (+DELTA_STEP);
                    if (e.Key.Key == ConsoleKey.DownArrow ) Walk   (-DELTA_STEP);
                }
            }
        }
    }
}
