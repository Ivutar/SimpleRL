using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E1M2
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E1M2 - Simple events loop";
            Util.Width = 40;
            Util.Height = 20;
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
