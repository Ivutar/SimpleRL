using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6.Screens
{
    class SettingsScreen : Screen
    {
        Menu main_menu;

        //system (console size, colors, formats and etc)
        //controls
        //game

        public SettingsScreen()
        {
            //main menu
            string[] main_menu_items = new string[] {
                "    System    ",
                "   Controls   ",
                "     Game     ",
            };
            int wd = main_menu_items.Select(i => i.Length).Sum();
            main_menu = new Menu(wd, 1, main_menu_items) { Horizontal = true, Focus = true };

            //system menu
            //...

            //controls menu
            //...

            //game menu
            //...
        }

        public override void Input(Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press)
            {
                if (e.Key.Key == ConsoleKey.Escape)
                {
                    ResetChanges();
                    Global.Sys.CurrentScreen = Global.Sys.MainMenu;
                }
                else if (e.Key.Key == ConsoleKey.Enter)
                {
                    AcceptChanges();
                    Global.Sys.CurrentScreen = Global.Sys.MainMenu;
                }
                else
                {
                    main_menu.Input(e);
                }
                //...
            }
        }

        public override void Draw()
        {
            Util.Buffer.Clear();

            //title
            CharInfo[] title = "{title}Settings{/}".Decorate(Global.decor, Color.LightGray, Color.Black);
            int titlex = (Util.Buffer.Width - title.Length) / 2;
            int titley = 1;
            Util.Buffer.Write(titlex, titley, title);

            //main menu
            int menux = (Util.Buffer.Width - main_menu.Width) / 2;
            int menuy = titley + 2;
            main_menu.Draw();
            Util.Buffer.Copy(main_menu, menux, menuy);

            //...

            //help
            CharInfo[] help = "{key}ESC{/}: abort changes   {key}←↑↓→{/}: change   {key}Enter{/}: accept changes".Decorate(Global.decor, Color.LightGray, Color.Black);
            int helpx = (Util.Buffer.Width - help.Length) / 2;
            int helpy = Util.Buffer.Height - 2;
            Util.Buffer.Write(helpx, helpy, help);

            Util.Swap();
        }    

        public void ResetChanges()
        {
            //...
        }

        public void AcceptChanges()
        {
            //...
        }
    }
}
