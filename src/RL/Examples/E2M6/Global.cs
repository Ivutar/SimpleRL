﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E2M6.Screens;
using RL;

namespace E2M6
{
    static class Global
    {
        //global vars
        public static Random rnd;
        public static Dictionary<string, ColorInfo> decor; //colored string decoration

        //grouped data containers
        public static DataGame Game;
        public static DataSys Sys;
        public static DataCfg Cfg;

        static Global()
        {
            rnd = new Random();
            decor = new Dictionary<string, ColorInfo>();
            decor["title"] = new ColorInfo { Fore = Color.LightRed };
            decor["key"] = new ColorInfo { Fore = Color.LightGreen };

            Game = new DataGame();
            Sys = new DataSys();
            Cfg = new DataCfg();
        }
    }
}
