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
        public static Random rnd;
        public static GameData Game;
        public static SysData Sys;

        static Global()
        {
            rnd = new Random();
            Game = new GameData();
            Sys = new SysData();
        }
    }
}
