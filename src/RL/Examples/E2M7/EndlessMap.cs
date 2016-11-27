using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpNoise;
using SharpNoise.Builders;
using System.Diagnostics;

namespace E2M7
{
    public struct Chunk
    {
        public const int Width = 32;
        public const int Height = 32;

        public Point pos;
        public int[,] data;
    }

    public interface IChunkBuilder
    {
        void Fill(int[,] chunk, Point pos);
    }

    public class NoiseBuilder : IChunkBuilder
    {
        PlaneNoiseMapBuilder builder;

        public NoiseBuilder(int seed)
        {
            var rnd = new SharpNoise.Modules.RidgedMulti() { Quality = NoiseQuality.Fast, Seed = seed };
            var scabi = new SharpNoise.Modules.ScaleBias() { Source0 = rnd, Scale = 8, Bias = 2 };
            var clamp = new SharpNoise.Modules.Clamp() { Source0 = scabi, LowerBound = -0.1, UpperBound = 15 };

            //builder
            builder = new PlaneNoiseMapBuilder()
            {
                DestNoiseMap = new NoiseMap(),
                SourceModule = clamp,
            };
            builder.SetDestSize(Chunk.Width, Chunk.Height);
        }

        public void Fill(int[,] chunk, Point pos)
        {
            Debug.Assert(chunk != null);
            Debug.Assert(chunk.GetLength(0) != Chunk.Width);
            Debug.Assert(chunk.GetLength(1) != Chunk.Height);

            builder.SetBounds(pos.X * Chunk.Width, (pos.X + 1) * Chunk.Width, pos.Y * Chunk.Height, (pos.Y + 1) * Chunk.Height);
            builder.Build();

            for (int x = 0; x < Chunk.Width; x++)
                for (int y = 0; y < Chunk.Height; y++)
                    chunk[x, y] = (int)builder.DestNoiseMap[x, y];
        }
    }

    public interface IChunkSerializer
    {
        bool Absent(Point pos);
        void Load(int[,] chunk, Point pos);
        void Unload(int[,] chunk, Point pos);
    }

    public class FakeSerializer : IChunkSerializer
    {
        public bool Absent(Point pos)
        {
            return true;
        }

        public void Load(int[,] chunk, Point pos)
        {
            //...
        }

        public void Unload(int[,] chunk, Point pos)
        {
            //...
        }
    }

    public class EndlessMap
    {
        public IChunkBuilder Builder { get; private set; }
        public IChunkSerializer Serializer { get; private set; }

        public Dictionary<Point, Chunk> data = new Dictionary<Point, Chunk>();

        int[,] current;

        Rectangle window;
        public Rectangle Window
        {
            get
            {
                return window;
            }
            set
            {
                //проверить новое окно
                //...

                //загрузить/сгенерировать чанки, выгрузить неиспользуемые чанки
                var old_keys = data.Keys; //todo: clone

                float x0 = (float)window.Left / Chunk.Width;
                float x1 = (float)window.Right / Chunk.Width;
                float y0 = (float)window.Top / Chunk.Height;
                float y1 = (float)window.Bottom / Chunk.Height;

                for (int x = (int)x0; x <= (int)x1; x++)
                    for (int y = (int)y0; y <= (int)y1; y++)
                    {
                        //...
                    }

                //заполнить current
                //...

                window = value;
            }
        }

        public int this [int x, int y]
        {
            get
            {
                if (current == null)
                    return 0;
                if (x < window.Left || x > window.Right || y < window.Top || y > window.Bottom)
                    return 0;
                return current[x - window.Left, y - window.Top];
            }
            set
            {
                //...
            }
        }

        public EndlessMap (Rectangle window, IChunkBuilder builder, IChunkSerializer serializer)
        {
            Builder = builder;
            Serializer = serializer;
            Window = window;
        }
    }
}
