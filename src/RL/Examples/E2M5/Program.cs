using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M5
{
    class Program
    {

        static void Main(string[] args)
        {
            Util.Title = "E2M5 - FOV";
            Util.Width = 80;
            Util.Height = 40;
            Util.CursorVisible = false;
            Maze maze = new Maze();

            while (true)
            {
                maze.Draw(32, 10, Util.Buffer);
                Util.Swap();

                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Press)
                {
                    if (e.Key.Key == ConsoleKey.Escape) break;
                    if (e.Key.Key == ConsoleKey.LeftArrow)  maze.Move(-1, 0);
                    if (e.Key.Key == ConsoleKey.RightArrow) maze.Move(1, 0);
                    if (e.Key.Key == ConsoleKey.UpArrow)    maze.Move(0, -1);
                    if (e.Key.Key == ConsoleKey.DownArrow)  maze.Move(0, 1);
                }
            }
            Console.ReadKey(true);
        }
    }
}
