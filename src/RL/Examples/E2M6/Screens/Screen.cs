using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6.Screens
{
    abstract class Screen
    {
        public abstract void Input(Event e);

        public abstract void Draw();
    }
}
