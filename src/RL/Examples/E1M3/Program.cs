using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E1M3
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E1M3 - Load canvas content";
            Util.Width = 40;
            Util.Height = 30;
            Util.CursorVisible = false;

            Canvas pic = Canvas.Load(@"..\..\screen.rl");
            Util.Buffer.Clear(new CharInfo(' ', Color.LightGray, Color.DarkGray));
            Util.Buffer.Copy(pic, 3, 3);
            Util.Swap();

            Console.ReadKey(true);
        }
    }
}
