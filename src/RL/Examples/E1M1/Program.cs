using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E1M1
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E1M1 - Hello world";
            Util.Width = 30;
            Util.Height = 20;
            Util.CursorVisible = false;

            Util.Buffer.Clear();
            Util.Buffer.Write(0, 0, "Hello world");
            Util.Swap();

            Console.ReadKey(true);
        }
    }
}
