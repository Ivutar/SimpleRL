﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E2M6 - Kobolds Cave";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            //main loop
            while (Global.Sys.Play)
            {
                Global.Sys.CurrentScreen.Draw();
                Global.Sys.CurrentScreen.Input(Events.GetNext(true));
            }
        }
    }
}
