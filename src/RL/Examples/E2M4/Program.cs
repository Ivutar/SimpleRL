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
        const double DELTA_ANGLE = 15 * Math.PI / 180; //rotation speed (15 degrees in radians)
        const double DELTA_STEP = 16; //walk speed
        const int CELL_SIZE = 64;

        static double player_x = CELL_SIZE * 1.5;
        static double player_y = CELL_SIZE * 1.5;
        static double player_angle = -270 * Math.PI / 180;

        static int[,] map = new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,0,0,0,0,0,1,0,1,1,0,0,1},
            {1,0,1,0,1,0,1,0,0,0,0,0,0,1,1},
            {1,0,0,0,1,0,1,0,1,1,0,1,1,1,1},
            {1,1,0,1,1,0,0,0,0,1,0,0,0,0,1},
            {1,0,0,0,1,0,1,1,1,1,1,1,0,1,1},
            {1,1,0,1,1,0,0,0,0,0,1,1,0,1,1},
            {1,0,0,0,0,0,1,0,1,0,1,0,0,0,1},
            {1,0,1,1,1,0,1,0,1,0,1,0,1,0,1},
            {1,0,1,0,1,1,1,0,1,0,1,0,1,0,1},
            {1,0,0,0,0,0,1,0,1,0,0,0,1,0,1},
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
            //...
        }

        static void Walk(double p)
        {
            //...
        }

        static void Rotate(double da)
        {
            player_angle += da;
            if (player_angle < 0) player_angle += 2 * Math.PI;
            if (player_angle >= 2 * Math.PI) player_angle -= 2 * Math.PI;
        }

        static void Main(string[] args)
        {
            Util.Title = "E2M4 - 3D Maze";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            while (true)
            {
                Draw();
                Util.Sleep(100);

                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Press)
                {
                    if (e.Key.Key == ConsoleKey.Escape) break;
                    if (e.Key.Key == ConsoleKey.LeftArrow) Rotate(-DELTA_ANGLE);
                    if (e.Key.Key == ConsoleKey.RightArrow) Rotate(+DELTA_ANGLE);
                    if (e.Key.Key == ConsoleKey.UpArrow) Walk(+DELTA_STEP);
                    if (e.Key.Key == ConsoleKey.DownArrow) Walk(-DELTA_STEP);
                }
            }
        }
    }
}
