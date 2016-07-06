using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E2M6 - Kobolds Cave";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            while (true)
            {
                //process input events
                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Key == ConsoleKey.Escape)
                    break;

                //update screen
                Util.Buffer.Clear();
                Util.Buffer.Write(0, 0, e.ToString());
                Util.Swap();
            }
        }
    }
}
