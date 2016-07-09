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
            int x, y;

            //create cave
            Cave = World.New(size);

            //dung
            for (int i = 0; i < Cave.Width; i++)
                for (int j = 0; j < Cave.Height; j++)
                {
                    Cell cell = Cave[i, j];
                    cell.Dung = Global.rnd.NextDouble() * 10;
                    Cave[i, j] = cell;
                }

            //fungus
            int cnt = Cave.Width * Cave.Height / 16;
            while (cnt-- > 0)
            {
                Cave.GetRandomEmptyCell(out x, out y);
                Cell cell = Cave[x, y];
                cell.Fungus = 10;
                Cave[x, y] = cell;
            }
            //for (int i = 0; i < 10; i++)
            //    UpdateFungus(); //some fungus grown

            //fill cave (monsters, items, bio, vegetation, traps and etc)
            //...

            //setup hero
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
                //todo: time system?

                //move
                Hero.X = newx;
                Hero.Y = newy;
                Cave.UpdateVisibility(Hero.X, Hero.Y, Hero.ViewRadius);

                //grown
                UpdateFungus();
            }
        }

        public void UpdateFungus()
        {
            for (int i = 0; i < Cave.Width; i++)
                for (int j = 0; j < Cave.Height; j++)
                    if (Cave[i, j].Fungus > 0)
                    {
                        ChangeFungus(i, j, 1); // normal grow
                        if (Cave[i,j].Dung > 0.1) //dung grow
                        {
                            double delta = Cave[i, j].Dung * 0.25;
                            ChangeDung(i, j, -delta);
                            ChangeFungus(i, j, +delta);
                        }
                        ExpandFungus(i, j);
                    }
        }

        void ExpandFungus(int x, int y)
        {
            if (Cave[x, y].Fungus > 4)
            {
                double delta = 0;

                if (Cave[x - 1, y].Kind == CellKind.Empty) { ChangeFungus(x - 1, y, 1); delta++; }
                if (Cave[x + 1, y].Kind == CellKind.Empty) { ChangeFungus(x + 1, y, 1); delta++; }
                if (Cave[x, y - 1].Kind == CellKind.Empty) { ChangeFungus(x, y - 1, 1); delta++; }
                if (Cave[x, y + 1].Kind == CellKind.Empty) { ChangeFungus(x, y + 1, 1); delta++; }

                ChangeFungus(x, y, -delta);
            }
        }

        void ChangeFungus(int x, int y, double delta)
        {
            Cell cell = Cave[x, y];
            cell.Fungus += delta;
            Cave[x, y] = cell;
        }

        void ChangeDung(int x, int y, double delta)
        {
            Cell cell = Cave[x, y];
            cell.Dung += delta;
            Cave[x, y] = cell;
        }
    }
}
