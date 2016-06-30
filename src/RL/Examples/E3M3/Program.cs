using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E3M3
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Title = "E3M3 - Colored string";
            Util.Width = 80;
            Util.Height = 25;
            Util.CursorVisible = false;

            var decor = new Dictionary<string, ColorInfo> ();
            decor["item"] = new ColorInfo { Fore = Color.LightRed };
            decor["gold"] = new ColorInfo { Fore = Color.Yellow };
            decor["help"] = new ColorInfo { Back = Color.Teal };
            decor["key"]  = new ColorInfo { Fore = Color.LightGreen };

            var str1 = "This chest contains {gold}{0}{/gold} gold coins. {help}You can {key}P{/key}ick or {key}D{/key}rop{/help}"
                .Decorate(decor, Color.LightGray, Color.Black, 100500);
            var str2 = "{item}{0,12}{/item} - cost {gold}{1,3}{/gold} gold."
                .Decorate(decor, Color.LightGray, Color.Black, "Long sword", 100);
            var str3 = "{item}{0,12}{/item} - cost {gold}{1,3}{/gold} gold."
                .Decorate(decor, Color.LightGray, Color.Black, "Helm", 75);
            var str4 = "{item}{0,12}{/item} - cost {gold}{1,3}{/gold} gold."
                .Decorate(decor, Color.LightGray, Color.Black, "Tower shield", 205);

            Util.Buffer.Clear();
            Util.Buffer.Write(2, 1, str1);
            Util.Buffer.Write(2, 3 + 0, str2);
            Util.Buffer.Write(2, 3 + 1, str3);
            Util.Buffer.Write(2, 3 + 2, str4);
            Util.Swap();

            Console.ReadKey(true);
        }
    }
}
