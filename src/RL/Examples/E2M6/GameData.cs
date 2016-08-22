﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    class GameData
    {
        public World Cave;

        public TimeSystem ActorUpdater;
        public PlayerActor Hero;
        public FungusActor Fungus;
        public List<Actor> Mobs;

        public GameData()
        {
            Mobs = new List<Actor>();
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
                    cell.Toxic = Global.rnd.NextDouble() * 10;
                    Cave[i, j] = cell;
                }

            //fungus
            int cnt = Cave.Width * Cave.Height / 16;
            while (cnt-- > 0)
            {
                Cave.GetRandomEmptyCell(out x, out y);
                Cell cell = Cave[x, y];
                cell.Fungus = Global.rnd.Next((int)Global.Cfg.FungusOld);
                Cave[x, y] = cell;
            }

            //fill cave (monsters, items, bio, vegetation, traps and etc)
            //...

            //setup hero
            Cave.GetRandomEmptyCell(out x, out y);
            Hero = new PlayerActor() { X = x, Y = y, Health = 100, ViewRadius = 10, Energy = 0, Speed = 100 };
            Cave.UpdateVisibility(Hero.X, Hero.Y, Hero.ViewRadius);

            //setup fungus
            Fungus = new FungusActor() { Energy = 0, Speed = 50 };

            //setup TimeSystem
            ActorUpdater = new TimeSystem();
            ActorUpdater.Actors.Add(Hero);
            ActorUpdater.Actors.Add(Fungus);
            ActorUpdater.Actors.AddRange(Mobs);
        }

        public void MoveHero(int dx, int dy)
        {
            int newx = Hero.X + dx;
            int newy = Hero.Y + dy;
            if (Cave[newx, newy].Kind == CellKind.Empty)
            {
                //move
                Hero.X = newx;
                Hero.Y = newy;
                Cave.UpdateVisibility(Hero.X, Hero.Y, Hero.ViewRadius);

                //time system
                ActorUpdater.UpdateActor(Hero, 1000);
                ActorUpdater.UpdateActors(Hero);
            }
        }
    }
}
