using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public class Menu : Widget
    {
        public List<string> Items { get; private set; }

        public Action<Menu, string, int> OnSelect;

        public Menu (int width, int height, IEnumerable<string> items): base (width, height)
        {
            Items = new List<string>(items);
        }

        public override bool Input(Event e)
        {
            //...
            return false;
        }

        public override void Draw()
        {
            //...
        }
    }
}
