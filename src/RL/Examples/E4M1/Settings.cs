using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E4M1
{
    public class Settings : Panel
    {
        TextEdit MapWidth;
        TextEdit MapHeight;
        TextEdit FillPercentage;
        TextEdit WallNeighbours;
        TextEdit PassNeighbours;
        TextEdit Steps;
        TextEdit Seed;

        Dictionary<string, ColorInfo> decor;

        public Action<Panel> OnGenerate;

        public Settings (int width, int height): base(width, height)
        {
            decor = new Dictionary<string, ColorInfo> ();
            decor["key"]  = new ColorInfo { Fore = Color.LightGreen, Back = Color.Teal };

            MapWidth       = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 0, Text = "200" };
            MapHeight      = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 1, Text = "200" };
            FillPercentage = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 2, Text = "0.4" };
            WallNeighbours = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 3, Text = "5" };
            PassNeighbours = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 4, Text = "3" };
            Steps          = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 5, Text = "2" };
            Seed           = new TextEdit(width - 2) { Left = 1, Top = 2 + 3 * 6, Text = "" };

            Widgets.Add(MapWidth);
            Widgets.Add(MapHeight);
            Widgets.Add(FillPercentage);
            Widgets.Add(WallNeighbours);
            Widgets.Add(PassNeighbours);
            Widgets.Add(Steps);
            Widgets.Add(Seed);

            FocusOn = Seed;

            BeforeInput = (p, w, e) =>
            { //add navigation: DOWN or TAB - next, UP or SHIFT+TAB - prev
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

            Fore = Color.LightGray;
            Back = Color.Teal;

            Draw();
        }

        public override bool Input(Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press && e.Key.Key == ConsoleKey.Spacebar)
            {
                if (OnGenerate != null)
                    OnGenerate(this);
                return true;
            }

            return base.Input(e);
        }

        public override void Draw()
        {
            base.Draw();

            //texts
            Color f = Color.White;
            Color b = Color.Teal;
            Write(1, 1 + 3 * 0, "Map width", f, b);
            Write(1, 1 + 3 * 1, "Map Height", f, b);
            Write(1, 1 + 3 * 2, "Fill precentage", f, b);
            Write(1, 1 + 3 * 3, "Create wall", f, b);
            Write(1, 1 + 3 * 4, "Remove wall", f, b);
            Write(1, 1 + 3 * 5, "Smoth steps", f, b);
            Write(1, 1 + 3 * 6, "Seed", f, b);

            Write(1, 1 + 3 * 7 + 0, "{key}Space{/} - regenerate".Decorate(decor, f, b));
            Write(1, 1 + 3 * 7 + 1, "{key}Esc{/} - exit".Decorate(decor, f, b));
            Write(1, 1 + 3 * 7 + 2, "{key}LB{/}+{key}mouse{/} - scroll".Decorate(decor, f, b));
        }
    }
}
