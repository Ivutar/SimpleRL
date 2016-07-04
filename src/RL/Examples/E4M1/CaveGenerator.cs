using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E4M1
{
    /// <summary>
    /// cave generator. see also: http://gamedevelopment.tutsplus.com/tutorials/generate-random-cave-levels-using-cellular-automata--gamedev-9664
    /// </summary>
    public class CaveGenerator
    {
        int[,] map;
        int[,] tmp;

        int width;
        int height;
        double fill;
        int wall;
        int pass;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public double Fill { get { return fill; } }
        public int Wall { get { return wall; } }
        public int Pass { get { return pass; } }
        public int Seed { get; set; }

        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return 1;
                return map[x, y];
            }
        }

        /// <summary>
        /// cave generator
        /// </summary>
        /// <param name="width">width >= 3</param>
        /// <param name="height">height >= 3</param>
        /// <param name="fill">[0-1], walls percentage</param>
        /// <param name="wall">wall will be created if neightbours >= wall</param>
        /// <param name="pass">wall will be removed if neightbours &lt;= pass</param>
        /// <param name="steps">smoth steps</param>
        public CaveGenerator(int width, int height, double fill, int wall, int pass, int steps, int? seed = null)
        {
            this.width = width < 3 ? 3 : width;
            this.height = height < 3 ? 3 : height;
            this.fill = fill;
            this.wall = wall;
            this.pass = pass;
            this.Seed = seed ?? new Random().Next();

            map = new int[Width, Height];
            tmp = new int[Width, Height];

            Start(steps);
        }

        public void Start(int steps)
        {
            Random rnd = new Random(Seed);

            //fill
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (rnd.NextDouble() <= fill)
                        map[x, y] = 1;
                    else
                        map[x, y] = 0;

            //smoth
            for (int i = 1; i <= steps; i++)
                Smoth();
        }

        public void Smoth()
        {
            //smoth
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    int n = Neightbours(x, y);
                    if (n >= Wall)
                        tmp[x, y] = 1;
                    else if (n <= Pass)
                        tmp[x, y] = 0;
                    else
                        tmp[x, y] = map[x, y];
                }

            //copy
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    map[x,y] = tmp[x, y];
        }

        int Neightbours(int x, int y)
        {
            int n = 0;
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                    if (x + dx < 0 || y + dy <= 0 || x + dx >= Width || y + dy >= Height)
                        n++;
                    else if (map[x + dx, y + dy] == 1)
                        n++;
            return n;
        }
    }
}
