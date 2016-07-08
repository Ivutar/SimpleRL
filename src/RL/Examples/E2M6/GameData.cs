using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    class GameData
    {
        public World Cave;
        public Player Hero;

        public GameData()
        {
        }

        public void StartNewGame(int size, int pop, int lvl, int hero_class)
        {
            //create cave
            Cave = World.New(size);

            //fill cave (monsters, items, bio, vegetation, traps and etc)
            //...

            //setup hero
            int x, y;
            Cave.GetRandomEmptyCell(out x, out y);
            Hero = new Player() { X = x, Y = y, Health = 100, ViewRadius = 10 };
            Cave.UpdateVisibility(Hero.X, Hero.Y, Hero.ViewRadius);
        }

        public void MoveHero(int dx, int dy)
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
