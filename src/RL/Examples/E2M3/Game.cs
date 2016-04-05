using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M3
{
    public class Game
    {
        int[,] map;
        int steps;
        int collectedcoins;
        int totalcoins;
        int lightradius;

        public int Width { get { return map.GetLength(0); } }
        public int Height { get { return map.GetLength(1); } }
        public int CollectedCoins { get { return collectedcoins; } }
        public int TotalCoins { get { return totalcoins; } }
        public int LightRadius { get { return lightradius; } }
        public int Steps { get { return steps; } }

        public Game (int width = 100, int height = 100, int totalcoins = 10, int lightradius = 5, int? seed = null)
        {
            //generate maze
            map = (new MazeGenerator(width, height)).Generate(seed);

            //...
        }

    }
}
