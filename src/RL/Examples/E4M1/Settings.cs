using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E4M1
{
    public class Settings : Panel
    {
        public Settings (int width, int height): base(width, height)
        {
            //...
        }

        public override bool Input(Event e)
        {
            //...
            return false;
        }

        public override void Draw()
        {
            Clear(new CharInfo(' ', Color.White, Color.DarkGray));
            //...
        }
    }
}
