using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E3M1
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E3M1 - Simple menu";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            Util.Buffer.Clear();
            Util.Buffer.Write(0, 0, "Simple menu");
            Util.Swap();

            Console.ReadKey(true);
        }
    }
}
