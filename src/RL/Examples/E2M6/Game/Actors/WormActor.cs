using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    class WormActor : LiveActor
    {
        public int Size { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override void Update(ref UpdateEvent e)
        {
            var Cave = Global.Game.Cave;
            int dx = Global.rnd.Next(-1, 2);
            int dy = Global.rnd.Next(-1, 2);

            //simple move logic
            if (Cave[X + dx, Y + dy].Kind == CellKind.Empty)
            {
                X += dx;
                Y += dy;

                //update move logic (attack, eat, and etc)
                //...
            }
        }
    }
}
