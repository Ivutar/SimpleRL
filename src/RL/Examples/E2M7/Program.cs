using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RL;
using SharpNoise;
using SharpNoise.Builders;

namespace E2M7
{
    class Program
    {
        static CharInfo[] tiles = new CharInfo[]
        {
            new CharInfo() { ch = ' ', back = RL.Color.Blue, fore = RL.Color.White }, // 0 - sea

            new CharInfo() { ch = ' ', back = RL.Color.Yellow, fore = RL.Color.Brown }, // 1 - sand 1
            new CharInfo() { ch = '░', back = RL.Color.Yellow, fore = RL.Color.Brown }, // 2 - sand 2
            new CharInfo() { ch = '▒', back = RL.Color.Yellow, fore = RL.Color.Brown }, // 3 - sand 3
            new CharInfo() { ch = '▓', back = RL.Color.Yellow, fore = RL.Color.Brown }, // 4 - sand 4
            new CharInfo() { ch = ' ', back = RL.Color.Brown, fore = RL.Color.Yellow }, // 5 - sand 5

            new CharInfo() { ch = ' ', back = RL.Color.Green, fore = RL.Color.LightGreen }, //  6 - grass 1
            new CharInfo() { ch = '░', back = RL.Color.Green, fore = RL.Color.LightGreen }, //  7 - grass 2
            new CharInfo() { ch = '▒', back = RL.Color.Green, fore = RL.Color.LightGreen }, //  8 - grass 3
            new CharInfo() { ch = '▓', back = RL.Color.Green, fore = RL.Color.LightGreen }, //  9 - grass 4
            new CharInfo() { ch = ' ', back = RL.Color.LightGreen, fore = RL.Color.Green }, // 10 - grass 5

            new CharInfo() { ch = ' ', back = RL.Color.DarkGray, fore = RL.Color.LightGray }, // 11 - rock 1
            new CharInfo() { ch = '░', back = RL.Color.DarkGray, fore = RL.Color.LightGray }, // 12 - rock 2
            new CharInfo() { ch = '▒', back = RL.Color.DarkGray, fore = RL.Color.LightGray }, // 13 - rock 3
            new CharInfo() { ch = '▓', back = RL.Color.DarkGray, fore = RL.Color.LightGray }, // 14 - rock 4
            new CharInfo() { ch = ' ', back = RL.Color.LightGray, fore = RL.Color.DarkGray }, // 15 - rock 5
        };

        static void Main(string[] args)
        {
            Util.Title = "E2M7 - Infinity Map";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            //rnd generator
            var rnd = new SharpNoise.Modules.RidgedMulti() { Quality = NoiseQuality.Fast };
            //var rnd = new SharpNoise.Modules.Perlin() { OctaveCount = 1, Frequency = 0.5, Persistence = 0.025 };
            var scabi = new SharpNoise.Modules.ScaleBias() { Source0 = rnd, Scale = 8, Bias = 2 };
            var clamp = new SharpNoise.Modules.Clamp() { Source0 = scabi, LowerBound = -0.1, UpperBound = 15 };

            //builder
            var builder = new PlaneNoiseMapBuilder()
            {
                DestNoiseMap = new NoiseMap(),
                SourceModule = clamp,
            };
            builder.SetDestSize(Util.Width, Util.Height); //4x4 -> 20x20
            float zoom = 0.4f;
            float aspect = (float)Util.Height / Util.Width; 
            var bounds = new RectangleF(0, 0, zoom, zoom * aspect);
            builder.SetBounds(bounds.Left, bounds.Right, bounds.Top, bounds.Bottom);
            builder.Build();

            //map
            for (int x = 0; x < Util.Width; x++)
                for (int y = 0; y < Util.Height; y++)
                {
                    int tileid = (int)Math.Truncate(builder.DestNoiseMap[x, y]);
                    CharInfo tile = tiles[tileid];
                    Util.Buffer[x, y] = tile;
                }

            Util.Swap();
        }
    }
}
