using RL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1M6
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E1M6 - Lines";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            while(true)
            {
                Util.Buffer.Clear();
                int wd = Util.Buffer.Width;
                int hg = Util.Buffer.Height;
                Random rnd = new Random();
                for (int i = 0; i < 20; i++)
                    Util.Buffer.Line(rnd.Next(wd), rnd.Next(hg), rnd.Next(wd), rnd.Next(hg), new CharInfo((char)('#' + rnd.Next(8)), (Color)rnd.Next(16), (Color)rnd.Next(16)));
                Util.Swap();

                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Press && e.Key.Key == ConsoleKey.Escape)
                    break;
            }

            Console.ReadKey(true);
        }
    }
}
