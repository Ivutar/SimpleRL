﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M2
{
    class Program
    {
        static int player_x = 1;
        static int player_y = 1;
        static int[,] map = (new MazeGenerator(80, 25, 5, 20)).Generate();

        static int MapHeight
        {
            get { return map.GetLength(1); }
        }

        static int MapWidth
        {
            get { return map.GetLength(0); }
        }

        static void Draw()
        {
            Util.Buffer.Clear();
            for (int y = 0; y < MapHeight; y++)
                for (int x = 0; x < MapWidth; x++)
                {
                    if (map[x, y] == 0) Util.Buffer.Write(x, y, " ");
                    if (map[x, y] == 1) Util.Buffer.Write(x, y, "#", Color.DarkGray, Color.Black);
                }
            Util.Buffer.Write(player_x, player_y, "@", Color.Yellow, Color.Black);
            Util.Swap();
        }

        static void Move(int dx, int dy)
        {
            int newx = player_x + dx;
            int newy = player_y + dy;
            if (newx >= 0 && newy >= 0 && newx < MapWidth && newy < MapHeight)
                if (map[newx, newy] == 0)
                {
                    player_x = newx;
                    player_y = newy;
                }
        }

        static void Main(string[] args)
        {
            Util.Title = "E2M2 - Random maze game";
            Util.Width = MapWidth;
            Util.Height = MapHeight;
            Util.CursorVisible = false;

            while (true)
            {
                Draw();

                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Press)
                {
                    if (e.Key.Key == ConsoleKey.Escape) break;
                    if (e.Key.Key == ConsoleKey.Spacebar) map = (new MazeGenerator(80, 25, 5, 20)).Generate();
                    if (e.Key.Key == ConsoleKey.LeftArrow) Move(-1, 0);
                    if (e.Key.Key == ConsoleKey.RightArrow) Move(1, 0);
                    if (e.Key.Key == ConsoleKey.UpArrow) Move(0, -1);
                    if (e.Key.Key == ConsoleKey.DownArrow) Move(0, 1);
                }
            }
        }
    }

}
