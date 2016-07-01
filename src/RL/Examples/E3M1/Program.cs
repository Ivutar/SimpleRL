using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E3M1
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E3M1 - Simple UI with textboxes";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            TextEdit edit1 = new TextEdit(5) { Text = "some_text", Back = Color.DarkGray, Focus = true, MaxLength = 5 };
            TextEdit edit2 = new TextEdit(16) { Text = "some very long text with very useless words", Back = Color.DarkGray };
            Widget current = edit1;
            edit1.Draw();
            edit2.Draw();

            while (true)
            {
                //update screen
                Util.Buffer.Clear();
                Util.Buffer.Copy(edit1, 2, 2);
                Util.Buffer.Copy(edit2, 2, 4);
                Util.Swap();

                //process input events
                Event e = Events.GetNext(true);

                //check exit events
                if (e.Kind == EventKind.Key && e.Key.Key == ConsoleKey.Escape)
                    break;

                //check textbox events
                if(!current.Input(e))
                    if (e.Kind == EventKind.Key && e.Key.Press)
                        if (e.Key.Key == ConsoleKey.Tab || e.Key.Key == ConsoleKey.UpArrow || e.Key.Key == ConsoleKey.DownArrow)
                        {
                            //switch current control
                            current.Focus = false;
                            if (current == edit1)
                                current = edit2;
                            else
                                current = edit1;
                            current.Focus = true;
                            edit1.Draw();
                            edit2.Draw();
                        }
            }

        }
    }
}
