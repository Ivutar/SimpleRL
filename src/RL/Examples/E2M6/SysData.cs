using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;
using E2M6.Screens;

namespace E2M6
{
    class SysData
    {
        public ControlManager Control;

        public Dictionary<string, ColorInfo> decor;

        public MainMenuScreen MainMenu;
        public NewGameScreen NewGame;
        public LoadScreen Load;
        public AchievementsScreen Achievements;
        public KoboldopediaScreen Koboldopedia;
        public HighscoresScreen Highscores;
        public SettingsScreen Settings;
        public ExploreScreen Explore;
        public Screen CurrentScreen;

        public bool Play { get; set; }

        public SysData ()
        {
            decor = new Dictionary<string, ColorInfo>();
            decor["title"] = new ColorInfo { Fore = Color.LightRed };
            decor["key"] = new ColorInfo { Fore = Color.LightGreen };

            Control = new ControlManager();

            //screens setup
            MainMenu = new MainMenuScreen();
            NewGame = new NewGameScreen();
            Load = new LoadScreen();
            Achievements = new AchievementsScreen();
            Koboldopedia = new KoboldopediaScreen();
            Explore = new ExploreScreen();
            Highscores = new HighscoresScreen();
            Settings = new SettingsScreen();
            CurrentScreen = MainMenu;

            //enable main loop
            Play = true;
        }
    }
}
