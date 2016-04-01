using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E1M4
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E1M4 - Mouse painter";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            while (true)
            {
                Event e = Events.GetNext(true);
                if (e.Kind == EventKind.Key && e.Key.Key == ConsoleKey.Escape)
                    break;
                if (e.Kind == EventKind.Mouse)
                    if (e.Mouse.Kind == MouseEventKind.Move || e.Mouse.Kind == MouseEventKind.Press)
                    {
                        if (e.Mouse.Left)
                        {
                            Util.Buffer.Write(e.Mouse.X, e.Mouse.Y, "#", Color.Yellow, Color.Black);
                            Util.Swap();
                        }
                        if (e.Mouse.Right)
                        {
                            Util.Buffer.Write(e.Mouse.X, e.Mouse.Y, " ");
                            Util.Swap();
                        }
                    }
            }
        }
    }
}
