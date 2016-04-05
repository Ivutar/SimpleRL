using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M3
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E2M3 - Extended maze game";
            Util.Width = Util.ReadConfigInt("Console.Width", 80);
            Util.Height = Util.ReadConfigInt("Console.Height", 25);
            Util.CursorVisible = false;

            Game game = new Game(
                Util.ReadConfigInt("Maze.Width", 100),
                Util.ReadConfigInt("Maze.Height", 100),
                Util.ReadConfigInt("Maze.Coins", 10),
                Util.ReadConfigInt("Maze.LightRadius", 5),
                Util.ReadConfigInt("Maze.Seed", 123)
                );

            while (true)
            {
                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Key == ConsoleKey.Escape)
                    break;
                //...
            }
        }
    }
}
