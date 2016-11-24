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
        static void Main(string[] args)
        {
            Util.Title = "E2M7 - Infinity Map";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            //rnd generator
            //var rnd = new SharpNoise.Modules.RidgedMulti() { Quality = NoiseQuality.Fast };
            var rnd = new SharpNoise.Modules.Perlin() { OctaveCount = 1, Frequency = 0.5, Persistence = 0.025 };
            var scabi = new SharpNoise.Modules.ScaleBias() { Source0 = rnd, Scale = 8, Bias = 2 };
            var clamp = new SharpNoise.Modules.Clamp() { Source0 = scabi, LowerBound = -0.1, UpperBound = 15 };

            //builder
            var builder = new PlaneNoiseMapBuilder()
            {
                DestNoiseMap = new NoiseMap(),
                SourceModule = clamp,
            };
            builder.SetDestSize(20, 20); //4x4 -> 20x20
            var bounds = new RectangleF(6, 1, 4, 4);
            builder.SetBounds(bounds.Left, bounds.Right, bounds.Top, bounds.Bottom);
            builder.Build();

            for(int x=0; x<20; x++)
                for (int y = 0; y < 20; y++)
                    Util.Buffer.Write(x * 3, y, string.Format("{0}", (int)Math.Truncate(builder.DestNoiseMap[x, y])));
            Util.Swap();

            //main loop
            //while (Global.Sys.Play)
            //{
            //    Global.Sys.CurrentScreen.Draw();
            //    Global.Sys.CurrentScreen.Input(Events.GetNext(true));
            //}
        }
    }
}
