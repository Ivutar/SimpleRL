using System;
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
        //...
        public static Dictionary<string, ColorInfo> decor;

        public static MainMenuScreen MainMenu;
        public static NewGameScreen NewGame;
        public static LoadScreen Load;
        public static AchievementsScreen Achievements;
        public static KoboldopediaScreen Koboldopedia;
        public static ExploreScreen Explore;
        public static Screen CurrentScreen;

        public static bool Play { get; set; }

        static Global()
        {
            decor = new Dictionary<string, ColorInfo>();
            decor["title"] = new ColorInfo { Fore = Color.LightRed };
            decor["key"] = new ColorInfo { Fore = Color.LightGreen };

            //screens setup
            MainMenu = new MainMenuScreen();
            NewGame = new NewGameScreen();
            Load = new LoadScreen();
            Achievements = new AchievementsScreen();
            Koboldopedia = new KoboldopediaScreen();
            Explore = new ExploreScreen();
            CurrentScreen = MainMenu;

            //enable main loop
            Play = true;

            //...
        }

    }
}
