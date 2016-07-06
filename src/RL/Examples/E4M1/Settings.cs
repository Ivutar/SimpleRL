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
        Random rnd = new Random();

        public TextEdit MapWidth;
        public TextEdit MapHeight;
        public TextEdit FillPercentage;
        public TextEdit WallNeighbours;
        public TextEdit PassNeighbours;
        public TextEdit Steps;
        public TextEdit Seed;

        Dictionary<string, ColorInfo> decor;

        public Action<Panel> OnGenerate;
        public Action<Panel> OnSave;

        public Settings (int width, int height): base(width, height)
        {
            decor = new Dictionary<string, ColorInfo> ();
            decor["key"]  = new ColorInfo { Fore = Color.LightGreen, Back = Color.Teal };

            MapWidth       = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 0, Text = "200" };
            MapHeight      = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 1, Text = "200" };
            FillPercentage = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 2, Text = "0.4" };
            WallNeighbours = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 3, Text = "5" };
            PassNeighbours = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 4, Text = "3" };
            Steps          = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 5, Text = "10" };
            Seed           = new TextEdit(width - 2) { Left = 1, Top = 1 + 3 * 6, Text = "" };

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
            if (e.Kind == EventKind.Key && e.Key.Press)
            {
                if (e.Key.Key == ConsoleKey.Spacebar)
                {
                    if (OnGenerate != null)
                        OnGenerate.Invoke(this);
                    return true;
                }
                else if (e.Key.Key == ConsoleKey.Enter)
                {
                    Seed.Text = rnd.Next().ToString();
                    if (OnGenerate != null)
                        OnGenerate.Invoke(this);
                }
                else if (e.Key.Key == ConsoleKey.F2)
                {
                    if (OnSave != null)
                        OnSave.Invoke(this);
                }
            }

            return base.Input(e);
        }

        public override void Draw()
        {
            base.Draw();

            //texts
            Color f = Color.White;
            Color b = Color.Teal;
            Write(1, 0 + 3 * 0, "Map width", f, b);
            Write(1, 0 + 3 * 1, "Map Height", f, b);
            Write(1, 0 + 3 * 2, "Fill precentage", f, b);
            Write(1, 0 + 3 * 3, "Create wall", f, b);
            Write(1, 0 + 3 * 4, "Remove wall", f, b);
            Write(1, 0 + 3 * 5, "Smoth steps", f, b);
            Write(1, 0 + 3 * 6, "Seed", f, b);

            Write(1, 0 + 3 * 7 + 0, "{key}Enter{/} new seed".Decorate(decor, f, b));
            Write(1, 0 + 3 * 7 + 1, "{key}Space{/} generate map".Decorate(decor, f, b));
            Write(1, 0 + 3 * 7 + 2, "{key}ESC{/} exit   {key}F2{/} save".Decorate(decor, f, b));
            Write(1, 0 + 3 * 7 + 3, "{key}LB{/}+{key}Mouse{/} scroll".Decorate(decor, f, b));
        }
    }
}
