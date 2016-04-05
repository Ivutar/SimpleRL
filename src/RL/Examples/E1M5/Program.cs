using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E1M5
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = Util.ReadConfigString("Title");
            Util.Width = Util.ReadConfigInt("Width", 80, 20, 120);
            Util.Height = Util.ReadConfigInt("Height", 25, 10, 40);
            Util.CursorVisible = Util.ReadConfigBool("CursorVisible");

            CharInfo ci = new CharInfo(
                Util.ReadConfigChar("Fill.Char", '+'),
                Util.ReadConfigColor("Fill.Fore", Color.White),
                Util.ReadConfigColor("Fill.Back", Color.Black)
            );

            Util.Buffer.Clear(ci);
            Util.Swap();

            Console.ReadKey(true);
        }
    }
}
