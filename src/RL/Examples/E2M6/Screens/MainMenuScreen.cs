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

            menu = new Menu(18, 4, new string[] {
                "       New        ",
                "       Load       ",
                "       About      ",
                "       Exit       ",
            })
            {
                Focus = true,
                Fore = Color.LightGray,
                Back = Color.Black,
                CurrentFore = Color.White,
                CurrentBack = Color.Teal,
                Left = (Util.Buffer.Width - 18) / 2,
                Top = (Util.Buffer.Height - 4) / 2,
            };

            menu.OnSelect = (sender, text, index) => {
                if (index == 0)
                {
                    //new
                    //...
                }
                else if (index == 1)
                {
                    //load
                    //...
                }
                else if (index == 2)
                {
                    //about
                    //...
                }
                else if (index == 3)
                {
                    //exit
                    Global.Play = false;
                }
            };
        }

        public override void Input(RL.Event e)
        {
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
            int x2 = Math.Max(titlex + title.Length, menux + menu.Width) + 1;
            int y2 = menuy + menu.Height + 1;

            Util.Buffer.Clear();

            //fone
            Util.Buffer.Copy(fone);
            Util.Buffer.FillRectangle(x1,y1,x2,y2, new CharInfo(' ', Color.White, Color.Black));

            //menu
            menu.Draw();
            Util.Buffer.Copy(menu, menux, menuy);

            //decor
            Util.Buffer.Write(titlex, titley, title);

            Util.Swap();
        }
    }
}
