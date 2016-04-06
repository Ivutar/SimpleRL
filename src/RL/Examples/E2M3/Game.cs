using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M3
{
    public class Game
    {
        //consts
        const int EMPTY = 0;
        const int WALL  = 1;
        const int COIN  = 2;

        //vars
        Random rnd;
        Canvas buffer;
        int[,] map;
        int steps;
        int collectedcoins;
        int totalcoins;
        int lightradius;
        int playerx;
        int playery;

        //props
        public int Width { get { return map.GetLength(0); } }
        public int Height { get { return map.GetLength(1); } }
        public int CollectedCoins { get { return collectedcoins; } }
        public int TotalCoins { get { return totalcoins; } }
        public int LightRadius { get { return lightradius; } }
        public int Steps { get { return steps; } }
        public int PlayerX { get { return playerx; } }
        public int PlayerY { get { return playery; } }

        public Canvas Buffer { get { return buffer; } }

        //ctor
        public Game (int width = 100, int height = 100, int totalcoins = 10, int lightradius = 5, int? seed = null)
        {
            width = width < 3 ? 3 : width;
            height = width < 3 ? 3 : width;

            //fill vars
            this.rnd = new Random(seed ?? 0);
            this.steps = 0;
            this.collectedcoins = 0;
            this.totalcoins = totalcoins < 0 ? 0 : totalcoins;
            this.lightradius = lightradius < 1 ? 1 : lightradius;

            //generate maze
            this.map = (new MazeGenerator(width, height)).Generate(seed);

            //add player
            RandomPos(ref playerx, ref playery);

            //add coins
            for (int i = 0; i < TotalCoins; i++)
            {
                int x = 1, y = 1;
                RandomPos(ref x, ref y);
                map[x,y] = COIN;
            }

            //fill buffer
            this.buffer = new Canvas(width, height);
            UpdateBuffer();
        }

        public void Move(int dx, int dy)
        {
            int newx = playerx + dx;
            int newy = playery + dy;

            if (newx >= 0 && newx < Width && newy >= 0 && newy < Height)
            {
                if (map[newx,newy] != WALL)
                {
                    playerx = newx;
                    playery = newy;
                    if (map[newx,newy] == COIN)
                    {
                        map[newx, newy] = EMPTY;
                        collectedcoins++;
                    }
                    UpdateBuffer();
                }
            }
        }

        void UpdateBuffer()
        {
            //clear
            buffer.Clear();

            //draw walls and coins
            for (int y=0; y<Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    int dist2 = (x - playerx) * (x - playerx) + (y - playery) * (y - playery);
                    if (dist2 >= lightradius*lightradius)
                        buffer.Write(x, y, " ", Color.White, Color.DarkGray);
                    else
                    {
                        if (map[x, y] == EMPTY) buffer.Write(x, y, " ");
                        if (map[x, y] == WALL) buffer.Write(x, y, "#", Color.LightGray);
                        if (map[x, y] == COIN) buffer.Write(x, y, "$", Color.Yellow);
                    }
                }
            
            //draw player
            buffer.Write(playerx, playery, "@", Color.LightRed);
        }

        bool RandomPos(ref int x, ref int y)
        {
            int resx = 0;
            int resy = 0;

            for (int i = 0; i < 1000; i++) //to avoid endless loop
            {
                resx = rnd.Next(1, Width - 1);
                resy = rnd.Next(1, Height - 1);

                if (map[resx, resy] == EMPTY)
                {
                    x = resx;
                    y = resy;
                    return true;
                }
            }

            return false;
        }

    }
}
