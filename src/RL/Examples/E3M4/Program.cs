using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E3M4
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E3M4 - Menu";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            Menu menu = new Menu(8, 4, new string[] { "  Play  ", "  Load  ", "  Save  ", "  Exit  " }) { Left = 2, Top = 2, Focus = true };
            menu.Draw();

            while (true)
            {
                //update screen
                Util.Buffer.Clear();
                Util.Buffer.Copy(menu, menu.Left, menu.Top);
                Util.Swap();

                //process input events
                Event e = Events.GetNext(true);

                //check exit events
                if (e.Kind == EventKind.Key && e.Key.Key == ConsoleKey.Escape)
                    break;

                //check textbox events
                menu.Input(e);
            }
        }
    }
}
