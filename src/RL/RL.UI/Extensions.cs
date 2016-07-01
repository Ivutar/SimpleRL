using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public static class Extensions
    {
        static public CharInfo[] Decorate(this string str, Dictionary<string, ColorInfo> decor, Color fore, Color back, params object[] args)
        {
            if (string.IsNullOrEmpty(str))
                return new CharInfo[0];

            if (decor == null)
                decor = new Dictionary<string, ColorInfo>();

            //"{item}{0,10}{/item} - cost {gold}{1}{/gold} gold."

            //hide decor tags
            string prepare = str;
            foreach (var key in decor.Keys)
                if (key != null && !string.IsNullOrWhiteSpace(key))
                {
                    prepare = prepare.Replace("{" + key + "}", "{{" + key + "}}");
                    prepare = prepare.Replace("{/" + key + "}", "{{/" + key + "}}");
                }
            prepare = prepare.Replace("{/}", "{{/}}");

            //format string
            prepare = String.Format(prepare, args);

            //paint string
            var output = new List<CharInfo>(); //result of painting
            var colors = new List<ColorInfo>(); //stack of colors
            colors.Add(new ColorInfo { Fore = fore, Back = back }); //default colors

            for (int i = 0; i < prepare.Length; i++)
            {
                bool tag = false;

                //check open tag
                foreach (string key in decor.Keys)
                    if (key != null && !string.IsNullOrWhiteSpace(key) && prepare.Substring(i).StartsWith("{" + key + "}"))
                    {
                        ColorInfo old = colors.Last();
                        ColorInfo cur = new ColorInfo { Fore = old.Fore, Back = old.Back };
                        if (decor[key].Fore != null) cur.Fore = decor[key].Fore;
                        if (decor[key].Back != null) cur.Back = decor[key].Back;
                        colors.Add(cur);
                        i += key.Length + 2;
                        tag = true;

                        break;
                    }

                //check close tag
                foreach (string key in decor.Keys)
                    if (key != null && !string.IsNullOrWhiteSpace(key) && prepare.Substring(i).StartsWith("{/" + key + "}"))
                    {
                        i += key.Length + 3;
                        if (colors.Count > 1)
                            colors.RemoveAt(colors.Count - 1);
                        tag = true;
                        break;
                    }
                if (prepare.Substring(i).StartsWith("{/}"))
                {
                    i += 3;
                    if (colors.Count > 1)
                        colors.RemoveAt(colors.Count - 1);
                    tag = true;
                }

                //all other chars
                if (tag)
                    i--;
                else
                    output.Add(new CharInfo(prepare[i], colors.Last().Fore ?? Color.White, colors.Last().Back ?? Color.Black));
            }

            return output.ToArray();
        }
    }

}
