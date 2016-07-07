using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL.FOV;

namespace E2M6
{
    enum CellKind
    {
        Empty = 0,
        Wall = 1,
        //...
    }

    struct Cell
    {
        public CellKind Kind;
        public bool Explored;
        public bool Visible;
    }

    class World
    {
        //data
        IFOV fov;
        Random rnd = new Random();
        Cell default_cell = new Cell() { Kind = CellKind.Wall, Explored = false, Visible = false };
        Cell[,] cells;

        //properties
        public int Width { get { return cells.GetLength(0); } }
        public int Height { get { return cells.GetLength(1); } }
        public Cell this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                    return default_cell;
                return cells[x, y];
            }
            set
            {
                if (x >= 0 && y >= 0 && x < Width && y < Height)
                    cells[x, y] = value;
            }
        }

        public World(int width, int height)
        {
            width = width < 3 ? 3 : width;
            height = height < 3 ? 3 : height;
            cells = new Cell[width, height];

            //fov setup
            fov = new RecursiveShadowCasting();

            fov.IsSolidBlock = (x, y) =>
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return true;
                return cells[x, y].Kind != CellKind.Empty;
            };

            fov.SetBlockLightDistanceSquared = (x, y, dist2) =>
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    return;
                cells[x, y].Explored = true;
                cells[x, y].Visible = true;
            };
        }

        public void UpdateVisibility (int x, int y, int radius)
        {
            //remove old visibility (depending old ViewRadius value)
            int limit = (int)fov.ViewRadius + 1;
            int wd = Width;
            int hg = Height;

            for (int i = x - limit; i <= x + limit; i++)
                for (int j = y - limit; j <= y + limit; j++)
                    if (i >= 0 && j >= 0 && i < wd && j < hg)
                        cells[i, j].Visible = false;

            //append visibility
            fov.ViewRadius = radius;
            fov.ComputeVisibility(x, y);
        }

        public void GetRandomEmptyCell (out int x, out int y)
        {
            int limit = 1000; //for avoid endless loop
            x = 0;
            y = 0;
            while (this[x,y].Kind != CellKind.Empty && --limit > 0)
            {
                x = rnd.Next(Width);
                y = rnd.Next(Height);
            }
        }

        public void Save (string filename)
        {
            //...
        }

        public static World Load(string filename)
        {
            //...
            return null;
        }

        public static World New (int size)
        {
            int width = 80 + 80 * size;
            int height = 25 + 25 * size;

            World world = new World(width, height);

            CaveGenerator generator = new CaveGenerator(width, height, 0.4, 5, 3, 10);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    world.cells[x, y].Kind = generator[x, y] == 0 ? CellKind.Empty : CellKind.Wall;

            return world;
        }
    }
}
