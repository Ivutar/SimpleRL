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
        static void Main(string[] args)
        {
            int smoth = 2;
            int width = 80;
            int height = 25;
            
            Util.Title = "E4M1 - Cellular cave generation";
            Util.Width = width;
            Util.Height = height;
            Util.CursorVisible = false;

            CaveGenerator gen = new CaveGenerator(width, height, 0.4, 5, 3, smoth);

            while(true)
            {
                //draw
                for (int x = 0; x < gen.Width; x++)
                    for (int y = 0; y < gen.Height; y++)
                        Util.Buffer.Write(x, y, gen[x, y] == 1 ? "#" : ".");
                Util.Buffer.Write(1, 1, string.Format(" Seed: {0} ", gen.Seed), Color.LightGreen, Color.Teal);
                Util.Swap();

                //input
                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Press)
                {
                    if (e.Key.Key == ConsoleKey.Escape)
                        break;
                    else
                    {
                        gen.Seed = new Random().Next();
                        gen.Start(smoth);                        
                    }
                }
            }
        }
    }
}
