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

            menu = new Menu(18, 7, new string[] {
                "  Start new game  ",
                " Continue playing ",
                "   Achievements   ",
                "   Koboldopedia   ",
                "    Highscores    ",
                "     Settings     ",
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
                    Global.Sys.CurrentScreen = Global.Sys.NewGame;
                }
                else if (index == 1)
                {
                    //Load game
                    Global.Sys.CurrentScreen = Global.Sys.Continue;
                }
                else if (index == 2)
                {
                    //Achievements
                    Global.Sys.CurrentScreen = Global.Sys.Achievements;
                }
                else if (index == 3)
                {
                    //Koboldopedia
                    Global.Sys.CurrentScreen = Global.Sys.Koboldopedia;
                }
                else if (index == 4)
                {
                    //highscores
                    Global.Sys.CurrentScreen = Global.Sys.Highscores;
                }
                else if (index == 5)
                {
                    //settings
                    Global.Sys.CurrentScreen = Global.Sys.Settings;
                }
                else if (index == 6)
                {
                    //Exit
                    Global.Sys.Play = false;
                }
            };
        }

        public override void Input(RL.Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press && e.Key.Key == ConsoleKey.Escape)
                Global.Sys.Play = false;
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
