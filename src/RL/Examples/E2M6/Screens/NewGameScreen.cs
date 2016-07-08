using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6.Screens
{
    class NewGameScreen : Screen
    {
        int current = 0;
        Menu[] menus;
        Menu cave_size_menu;
        Menu popuplation_menu;
        Menu level_menu;
        Menu class_menu;

        public NewGameScreen ()
        {
            int wd = Util.Buffer.Width - 2;

            //cave size
            cave_size_menu = new Menu(wd, 1, new string[] {
                "   Tiny   ",
                "   Small  ",
                "  Medium  ",
                "   Large  ",
                "   Huge   " })
            { Horizontal = true, Focus = true };

            //cave population
            popuplation_menu = new Menu(wd, 1, new string[] {
                "   Small  ",
                "  Medium  ",
                "   Large  "}) { Horizontal = true, Focus = true };

            //starting level
            level_menu = new Menu(wd, 1, new string[] {
                "   1 lvl  ",
                "   5 lvl  ",
                "  10 lvl  ",
                "  15 lvl  ",
                "  20 lvl  " }) { Horizontal = true, Focus = true };

            //hero class
            class_menu = new Menu(wd, 1, new string[] {
                "  Peasant ",
                "   Rogue  ",
                "  Warrior ",
                "  Archer  ",
                "   Mage   " }) { Horizontal = true, Focus = true };

            menus = new Menu[] { cave_size_menu, popuplation_menu, level_menu, class_menu };
            UpdateMenuColors();
        }

        public override void Input(Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press)
            {
                if (e.Key.Key == ConsoleKey.Escape)
                {
                    Global.Sys.CurrentScreen = Global.Sys.MainMenu;
                }
                else if (e.Key.Key == ConsoleKey.Enter) 
                {
                    //start new game
                    Global.Game.StartNewGame(cave_size_menu.Current, popuplation_menu.Current, level_menu.Current, class_menu.Current);
                    Global.Sys.CurrentScreen = Global.Sys.Explore;
                    Global.Sys.Explore.UpdateView(); //center view on hero
                }
                else if (e.Key.Key == ConsoleKey.UpArrow)
                {
                    current = current <= 0 ? menus.Length - 1 : current - 1;
                    UpdateMenuColors();
                }
                else if (e.Key.Key == ConsoleKey.DownArrow)
                {
                    current = current >= menus.Length - 1 ? 0 : current + 1;
                    UpdateMenuColors();
                }
                else
                {
                    menus[current].Input(e);
                }
            }
        }

        public override void Draw()
        {
            Util.Buffer.Clear();

            //menu
            Color title = Color.Heaven;
            int menuwd = 5 * 10;
            int menuhg = menus.Length * 4;
            int left = (Util.Buffer.Width - menuwd) / 2;
            int top = (Util.Buffer.Height - menuhg) / 2;
            cave_size_menu.Draw();
            popuplation_menu.Draw();
            level_menu.Draw();
            class_menu.Draw();

            Util.Buffer.Write(left - 1, top + 4 * 0 - 2, "Cave size",       title);
            Util.Buffer.Write(left - 1, top + 4 * 1 - 2, "Cave population", title);
            Util.Buffer.Write(left - 1, top + 4 * 2 - 2, "Starting level",  title);
            Util.Buffer.Write(left - 1, top + 4 * 3 - 2, "Hero class",      title);

            Util.Buffer.Copy(cave_size_menu,   left, top + 4 * 0);
            Util.Buffer.Copy(popuplation_menu, left, top + 4 * 1);
            Util.Buffer.Copy(level_menu,       left, top + 4 * 2);
            Util.Buffer.Copy(class_menu,       left, top + 4 * 3);

            //help
            CharInfo[] help = "{key}ESC{/}: main menu   {key}←↑↓→{/}: change   {key}Enter{/}: start new game".Decorate(Global.Sys.decor, Color.LightGray, Color.Black);
            int helpx = (Util.Buffer.Width - help.Length) / 2;
            int helpy = Util.Buffer.Height - 2;
            Util.Buffer.Write(helpx, helpy, help);

            Util.Swap();
        }

        void UpdateMenuColors()
        {
            for (int i=0; i<menus.Length; i++)
            {
                menus[i].Fore = Color.DarkGray;
                menus[i].CurrentBack = Color.DarkGray;
            }

            menus[current].Fore = Color.White;
            menus[current].CurrentBack = Color.White;
        }

    }
}
