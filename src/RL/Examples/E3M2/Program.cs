using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E3M2
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E3M1 - Panel Example";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            //setup panel
            Panel panel = new Panel(20, 20, Util.Buffer) { Left = 2, Top = 3, Back = Color.Teal };
            panel.Widgets.Add(new TextEdit(18) { Left = 1, Top = 1, Text = "text1" });
            panel.Widgets.Add(new TextEdit(18) { Left = 1, Top = 3, Text = "text22" });
            panel.Widgets.Add(new TextEdit(18) { Left = 1, Top = 5, Text = "text333" });
            panel.FocusOn = panel.Widgets[0];            
            panel.BeforeInput = (p, w, e) => { //add navigation: DOWN or TAB - next, UP or SHIFT+TAB - prev
                if (e.Kind == EventKind.Key && e.Key.Press && (e.Key.Key == ConsoleKey.Tab || e.Key.Key == ConsoleKey.UpArrow || e.Key.Key == ConsoleKey.DownArrow))
                {
                    if ((e.Key.Shift && e.Key.Key == ConsoleKey.Tab) || e.Key.Key == ConsoleKey.UpArrow)
                        p.FocusOn = p.Prev(w);
                    else
                        p.FocusOn = p.Next(w);
                    return true;
                }
                return false;
            };
            panel.Draw();

            while (true)
            {
                //update screen
                Util.Buffer.Clear();
                Util.Buffer.Write(panel.Left, panel.Top - 2, "Use UP/DOWN or TAB[+SHIFT] for navigation");
                panel.Draw();
                Util.Swap();

                //process input events
                Event e = Events.GetNext(true);

                //check exit events
                if (e.Kind == EventKind.Key && e.Key.Key == ConsoleKey.Escape)
                    break;

                //check textbox events
                panel.Input(e);
            }
        }
    }
}
