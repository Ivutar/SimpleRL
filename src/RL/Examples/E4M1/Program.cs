using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E4M1
{
    class Program
    {
        static void Gen2Map (CaveGenerator gen, Canvas map)
        {
            for (int x = 0; x < gen.Width; x++)
                for (int y = 0; y < gen.Height; y++)
                    map.Write(x, y, gen[x, y] == 1 ? "#" : ".", Color.DarkGray, Color.Black);
        }

        static void Main(string[] args)
        {
            int left = 20;
            int screen_width = 80;
            int screen_height = 25;
            int map_width = 200;
            int map_height = 200;
            int smoth = 10;
            int seed = new Random().Next();

            Util.Title = "E4M1 - Cellular cave generation";
            Util.Width = screen_width;
            Util.Height = screen_height;
            Util.CursorVisible = false;

            CaveGenerator gen = new CaveGenerator(map_width, map_height, 0.4, 5, 3, smoth, seed);

            Viewer viewer = new Viewer(screen_width - left, screen_height) { DX = -1, DY = -1 };
            viewer.Content = new Canvas(map_width, map_height);
            Gen2Map(gen, viewer.Content);

            Settings settings = new Settings(left, screen_height);
            settings.Seed.Text = seed.ToString();
            settings.OnGenerate = (panel) =>
            {
                int    wd    = 200;
                int    hg    = 200;
                double f     = 0.4;
                int    w     = 5;
                int    p     = 3;
                int    steps = smoth;
                int    sd    = new Random().Next();

                try { wd    = int.Parse(settings.MapWidth.Text);          } catch { }
                try { hg    = int.Parse(settings.MapHeight.Text);         } catch { }
                try { f     = double.Parse(settings.FillPercentage.Text); } catch { }
                try { w     = int.Parse(settings.WallNeighbours.Text);    } catch { }
                try { p     = int.Parse(settings.PassNeighbours.Text);    } catch { }
                try { steps = int.Parse(settings.Steps.Text);             } catch { }
                try { sd    = int.Parse(settings.Seed.Text);              } catch { }

                ((Settings)panel).MapWidth.Text = wd.ToString();
                ((Settings)panel).MapHeight.Text = hg.ToString();
                ((Settings)panel).FillPercentage.Text = f.ToString();
                ((Settings)panel).WallNeighbours.Text = w.ToString();
                ((Settings)panel).PassNeighbours.Text = p.ToString();
                ((Settings)panel).Steps.Text = steps.ToString();
                ((Settings)panel).Seed.Text = sd.ToString();

                viewer.Content.Width = wd;
                viewer.Content.Height = hg;

                gen = new CaveGenerator(wd, hg, f, w, p, steps, sd);

                Gen2Map(gen, viewer.Content);
            };
            settings.OnSave = (panel) =>
            {
                viewer.Content.Save(settings.Seed.Text + ".rl");
            };
            
            while (true)
            {
                //draw
                settings.Draw();
                viewer.Draw();
                Util.Buffer.Copy(settings, 0, 0);
                Util.Buffer.Copy(viewer, left, 0);
                Util.Swap();

                //input
                Event e = Events.GetNext(true);
                viewer.Input(e);
                settings.Input(e);
                if (e.Kind == EventKind.Key && e.Key.Press)
                    if (e.Key.Key == ConsoleKey.Escape)
                        break;
            }
        }
    }
}
