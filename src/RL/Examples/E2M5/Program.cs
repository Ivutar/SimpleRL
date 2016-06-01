using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M5
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E2M5 - FOV";
            Util.Width = 80;
            Util.Height = 40;
            Util.CursorVisible = false;

            Util.Buffer.Clear();
            Util.Buffer.Write(2, 2, "Field Of View - example");
            Util.Swap();

            Console.ReadKey(true);
        }
    }
}
