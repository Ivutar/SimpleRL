using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M3
{
    public class MazeGenerator
    {
        const int DIRT      = 1;
        const int WALL      = 2;
        const int HARD_WALL = 3; //unbreakable
        const int EMPTY     = 4;

        struct Point
        {
            public int x;
            public int y;
        }

        #region [ Variables ]

        Random rnd;

        int[,] map;

        #endregion

        #region [ Properties ]

        int width = 3;
        public int Width
        {
            get { return width; }
            set { width = value > 3 ? value : 3; }
        }

        int height = 3;
        public int Height
        {
            get { return height; }
            set { height = value > 3 ? value : 3; }
        }

        int? min = null;
        public int? Min
        {
            get { return min; }
            set { min = value > 1 ? value : null; }
        }

        int? max = null;
        public int? Max
        {
            get { return max; }
            set { max = value > 2 ? value : null; }
        }

        #endregion

        #region [ Helpers ]

        int UpdateTileType(int type)
        {
            if (type == EMPTY) return EMPTY;
            if (type == DIRT) return WALL;
            return HARD_WALL; //by default;
        }

        bool DiagonalCorner(int x, int y)
        {
            //##?
            //.x#
            //##?
            if (map[x - 1, y] == EMPTY && (map[x + 1, y - 1] == EMPTY || map[x + 1, y + 1] == EMPTY)) return true;

            //?##
            //#x.
            //?##
            if (map[x + 1, y] == EMPTY && (map[x - 1, y - 1] == EMPTY || map[x - 1, y + 1] == EMPTY)) return true;

            //#.#
            //#x#
            //?#?
            if (map[x, y - 1] == EMPTY && (map[x - 1, y + 1] == EMPTY || map[x + 1, y + 1] == EMPTY)) return true;

            //?#?
            //#x#
            //#.#
            if (map[x, y + 1] == EMPTY && (map[x - 1, y - 1] == EMPTY || map[x + 1, y - 1] == EMPTY)) return true;

            return false;
        }

        bool AnyWalls()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (map[x, y] == WALL && !DiagonalCorner(x,y))
                        return true;

            return false;
        }

        void ChooseRandomWall(out int posx, out int posy)
        {
            List<Point> walls = new List<Point>();

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (map[x, y] == WALL && !DiagonalCorner(x, y))
                        walls.Add(new Point() { x = x, y = y });

            Point wall = walls[rnd.Next(walls.Count)];
            posx = wall.x;
            posy = wall.y;
        }

        void Dig(int x, int y)
        {
            if (x <= 0 || y <= 0 || x >= Width - 1 || y >= Height - 1)
                return;

            map[x, y] = EMPTY;

            map[x - 1, y] = UpdateTileType(map[x - 1, y]);
            map[x + 1, y] = UpdateTileType(map[x + 1, y]);
            map[x, y - 1] = UpdateTileType(map[x, y - 1]);
            map[x, y + 1] = UpdateTileType(map[x, y + 1]);
        }

        bool CanDig(int x, int y)
        {
            int walls = 0;
            if (map[x - 1, y] == WALL && !DiagonalCorner(x - 1, y)) walls++;
            if (map[x + 1, y] == WALL && !DiagonalCorner(x + 1, y)) walls++;
            if (map[x, y - 1] == WALL && !DiagonalCorner(x, y - 1)) walls++;
            if (map[x, y + 1] == WALL && !DiagonalCorner(x, y + 1)) walls++;
            return walls > 0;
        }

        void Move(int currentx, int currenty, out int newx, out int newy)
        {
            List<Point> dir = new List<Point>();

            if (map[currentx - 1, currenty] == WALL && !DiagonalCorner(currentx - 1, currenty)) dir.Add(new Point() { x = currentx - 1, y = currenty });
            if (map[currentx + 1, currenty] == WALL && !DiagonalCorner(currentx + 1, currenty)) dir.Add(new Point() { x = currentx + 1, y = currenty });
            if (map[currentx, currenty - 1] == WALL && !DiagonalCorner(currentx, currenty - 1)) dir.Add(new Point() { x = currentx, y = currenty - 1 });
            if (map[currentx, currenty + 1] == WALL && !DiagonalCorner(currentx, currenty + 1)) dir.Add(new Point() { x = currentx, y = currenty + 1 });

            if (dir.Count <= 0)
            {
                newx = 0;
                newy = 0;
                return;
            }

            Point wall = dir[rnd.Next(dir.Count)];
            newx = wall.x;
            newy = wall.y;
        }

        #endregion

        public MazeGenerator(int width, int height, int? min = null, int? max = null)
        {
            Width = width;
            Height = height;
            Min = min;
            Max = max;
        }

        public int[,] Generate (int? seed = null)
        {
            //init random generator
            if (seed != null)
                rnd = new Random(seed ?? 0);
            else
                rnd = new Random();

            //initial map fill 
            map = new int[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                        map[x, y] = HARD_WALL;
                    else
                        map[x, y] = DIRT;

            //start
            Dig(rnd.Next(1, Width - 1), rnd.Next(1, Height - 1));

            //run worm 
            while (AnyWalls())
            {
                //choose worm start poition
                int currentx = -1;
                int currenty = -1;
                ChooseRandomWall(out currentx, out currenty);
                Dig(currentx, currenty);

                //digging
                int a = Min ?? 0;
                int b = Max ?? int.MaxValue;
                int max_length = rnd.Next(a, b);
                for (int i=0; i<max_length; i++)
                {
                    if (!CanDig(currentx, currenty))
                        break;

                    int newx = -1;
                    int newy = -1;
                    Move(currentx, currenty, out newx, out newy);
                    currentx = newx;
                    currenty = newy;
                    Dig(currentx, currenty);
                }
            }

            //return result
            int[,] res = new int[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (map[x, y] == EMPTY)
                        res[x, y] = 0;
                    else
                        res[x, y] = 1;
            return res;
        }
    }
}
