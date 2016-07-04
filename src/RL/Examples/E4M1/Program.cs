using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E4M1
{
    class Program
    {
        static void Gen2Map (CaveGenerator gen, Canvas map)
        {
            for (int x = 0; x < gen.Width; x++)
                for (int y = 0; y < gen.Height; y++)
                    map.Write(x, y, gen[x, y] == 1 ? "#" : ".", Color.DarkGray, Color.Black);
        }

        static void Main(string[] args)
        {
            int left = 20;
            int screen_width = 80;
            int screen_height = 25;
            int map_width = 200;
            int map_height = 200;
            int smoth = 2;

            Util.Title = "E4M1 - Cellular cave generation";
            Util.Width = screen_width;
            Util.Height = screen_height;
            Util.CursorVisible = false;

            CaveGenerator gen = new CaveGenerator(map_width, map_height, 0.4, 5, 3, smoth);
            Settings settings = new Settings(left, screen_height);
            Viewer viewer = new Viewer(screen_width - left, screen_height) { DX = left };
            viewer.Content = new Canvas(map_width, map_height);
            Gen2Map(gen, viewer.Content);

            while (true)
            {
                //draw
                settings.Draw();
                viewer.Draw();
                Util.Buffer.Copy(settings, 0, 0);
                Util.Buffer.Copy(viewer, left, 0);
                Util.Swap();

                //input
                Event e = Events.GetNext(true);
                viewer.Input(e);
                if (e.Kind == EventKind.Key && e.Key.Press)
                {
                    if (e.Key.Key == ConsoleKey.Escape)
                        break;
                    else
                    {
                        gen.Seed = new Random().Next();
                        gen.Start(smoth);
                        Gen2Map(gen, viewer.Content);
                    }
                }
            }
        }
    }
}
