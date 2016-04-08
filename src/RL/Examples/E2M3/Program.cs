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
        static int offsetx = Util.ReadConfigInt("View.X", 1);
        static int offsety = Util.ReadConfigInt("View.Y", 1);
        static int infox = Util.ReadConfigInt("Info.X", 1);
        static int infoy = Util.ReadConfigInt("Info.Y", 1);

        static void Draw(Game game)
        {
            Util.Buffer.Clear(new CharInfo(' ', Color.Black, Color.LightGray));
            Util.Buffer.Write(infox, infoy, string.Format("Conis: {0}/{1}", game.CollectedCoins, game.TotalCoins), Color.Black, Color.LightGray);
            Util.Buffer.Write(infox, infoy+2, string.Format("Steps: {0}", game.Steps), Color.Black, Color.LightGray);

            Util.Buffer.Copy(game.Buffer, offsetx, offsety);
            Util.Swap();
        }

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
                Util.ReadConfigInt("Maze.LightRadius", 6),
                Util.ReadConfigInt("Maze.Seed", 123)
                );

            Draw(game);
            while (true)
            {
                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Press == true)
                {
                    if (e.Key.Key == ConsoleKey.Escape) break;
                    if (e.Key.Key == ConsoleKey.LeftArrow)  game.Move(-1,  0);
                    if (e.Key.Key == ConsoleKey.RightArrow) game.Move(+1,  0);
                    if (e.Key.Key == ConsoleKey.UpArrow)    game.Move( 0, -1);
                    if (e.Key.Key == ConsoleKey.DownArrow)  game.Move( 0, +1);
                }

                Draw(game);
            }
        }
    }
}
