﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6.Screens
{
    class ContinueScreen : Screen
    {
        public ContinueScreen()
        {
            //...
        }

        public override void Input(Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press)
            {
                if (e.Key.Key == ConsoleKey.Escape)
                {
                    Global.Sys.CurrentScreen = Global.Sys.MainMenu;
                }
                //...
            }
        }

        public override void Draw()
        {
            Util.Buffer.Clear();

            //title
            CharInfo[] title = "{title}Continue playing{/}".Decorate(Global.decor, Color.LightGray, Color.Black);
            int titlex = (Util.Buffer.Width - title.Length) / 2;
            int titley = 1;
            Util.Buffer.Write(titlex, titley, title);

            //...

            //help
            CharInfo[] help = "{key}ESC{/}: main menu".Decorate(Global.decor, Color.LightGray, Color.Black);
            int helpx = (Util.Buffer.Width - help.Length) / 2;
            int helpy = Util.Buffer.Height - 2;
            Util.Buffer.Write(helpx, helpy, help);

            Util.Swap();
        }
    }
}
