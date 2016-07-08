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
        ///////////////
        // game data //
        ///////////////
        public static World Cave;
        public static Player Hero;
        public static ControlManager Control;

        ////////////////////////
        // app infrastructure //
        ////////////////////////
        public static Dictionary<string, ColorInfo> decor;

        public static MainMenuScreen MainMenu;
        public static NewGameScreen NewGame;
        public static LoadScreen Load;
        public static AchievementsScreen Achievements;
        public static KoboldopediaScreen Koboldopedia;
        public static HighscoresScreen Highscores;
        public static SettingsScreen Settings;
        public static ExploreScreen Explore;
        public static Screen CurrentScreen;

        public static bool Play { get; set; }


        static Global()
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

        public static void StartNewGame (int size, int pop, int lvl, int hero_class)
        {
            //create cave
            Global.Cave = World.New(size);

            //fill cave (monsters, items, bio, vegetation, traps and etc)
            //...

            //setup hero
            int x, y;
            Cave.GetRandomEmptyCell(out x, out y);
            Hero = new Player() { X = x, Y = y, Health = 100, ViewRadius = 10 };
            Cave.UpdateVisibility(Hero.X, Hero.Y, Hero.ViewRadius);
        }

        public static void MoveHero (int dx, int dy)
        {
            int newx = Hero.X + dx;
            int newy = Hero.Y + dy;
            if (Cave[newx, newy].Kind == CellKind.Empty)
            {
                Hero.X = newx;
                Hero.Y = newy;
                Cave.UpdateVisibility(Hero.X, Hero.Y, Hero.ViewRadius);
            }
        }
    }
}
