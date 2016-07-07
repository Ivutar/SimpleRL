using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6.Screens
{
    class MainMenuScreen : Screen
    {
        Canvas fone;
        Menu menu;

        public MainMenuScreen ()
        {
            fone = Canvas.Load(@"..\..\fone.rl");

            menu = new Menu(18, 5, new string[] {
                "  Start new game  ",
                "     Load game    ",
                "   Achievements   ",
                "   Koboldopedia   ",
                "       Exit       ",
            })
            {
                Focus = true,
                Fore = Color.LightGray,
                Back = Color.Black,
                CurrentFore = Color.White,
                CurrentBack = Color.Black,
                Left = (Util.Buffer.Width - 18) / 2,
                Top = (Util.Buffer.Height - 4) / 2,
            };

            menu.OnSelect = (sender, text, index) => {
                if (index == 0)
                {
                    //Start new game
                    //... reset new game screen
                    Global.CurrentScreen = Global.NewGame;
                }
                else if (index == 1)
                {
                    //Load game
                    Global.CurrentScreen = Global.Load;
                }
                else if (index == 2)
                {
                    //Achievements
                    Global.CurrentScreen = Global.Achievements;
                }
                else if (index == 3)
                {
                    //Koboldopedia
                    Global.CurrentScreen = Global.Koboldopedia;
                }
                else if (index == 4)
                {
                    //Exit
                    Global.Play = false;
                }
            };
        }

        public override void Input(RL.Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press && e.Key.Key == ConsoleKey.Escape)
                Global.Play = false;
            else
                menu.Input(e);
        }

        public override void Draw()
        {
            CharInfo[] title = "{title}Kobolds Cave{/title}".Decorate(Global.decor, Color.White, Color.Black);
            int menux = menu.Left;
            int menuy = menu.Top;
            int titlex = (Util.Buffer.Width - title.Length) / 2;
            int titley = menuy - 2;
            int x1 = Math.Min(titlex, menux) - 1;
            int y1 = titley - 1;
            int x2 = Math.Max(titlex + title.Length, menux + menu.Width);
            int y2 = menuy + menu.Height;

            Util.Buffer.Clear();

            //fone
            Util.Buffer.Copy(fone);
            Util.Buffer.FillRectangle(x1,y1,x2,y2, new CharInfo(' ', Color.White, Color.Black));

            //menu
            menu.Draw();
            Util.Buffer.Copy(menu, menux, menuy);

            //title
            Util.Buffer.Write(titlex, titley, title);

            //help
            CharInfo[] help = " {key}ESC{/}: quit   {key}↑↓{/}: change   {key}Enter{/}: select ".Decorate(Global.decor, Color.LightGray, Color.Black);
            int helpx = (Util.Buffer.Width - help.Length) / 2;
            int helpy = Util.Buffer.Height - 2;
            Util.Buffer.Write(helpx, helpy, help);

            Util.Swap();
        }
    }
}
