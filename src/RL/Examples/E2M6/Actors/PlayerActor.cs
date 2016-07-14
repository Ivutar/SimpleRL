using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    class PlayerActor : Actor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int ViewRadius { get; set; }

        //todo: exp, lvl, class, inventory and so on

        public override void Update(ref UpdateEvent e)
        {
            //...
        }
    }
}
