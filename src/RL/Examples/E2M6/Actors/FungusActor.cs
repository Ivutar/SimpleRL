using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6.Actors
{
    class FungusActor : Actor
    {
        public override void Update(ref UpdateEvent e)
        {
            var Cave = Global.Game.Cave;

            for (int i = 0; i < Cave.Width; i++)
                for (int j = 0; j < Cave.Height; j++)
                    if (Cave[i, j].Fungus > 0)
                    {
                        ChangeFungus(i, j, 1); // normal grow
                        if (Cave[i, j].Toxic > 0.1) //dung grow
                        {
                            double delta = Cave[i, j].Toxic * 0.25;
                            ChangeDung(i, j, -delta);
                            ChangeFungus(i, j, +delta);
                        }
                        ExpandFungus(i, j);
                    }
        }

        void ExpandFungus(int x, int y)
        {
            if (Global.Game.Cave[x, y].Fungus > 4)
            {
                double delta = 0;

                if (Global.Game.Cave[x - 1, y].Kind == CellKind.Empty) { ChangeFungus(x - 1, y, 1); delta++; }
                if (Global.Game.Cave[x + 1, y].Kind == CellKind.Empty) { ChangeFungus(x + 1, y, 1); delta++; }
                if (Global.Game.Cave[x, y - 1].Kind == CellKind.Empty) { ChangeFungus(x, y - 1, 1); delta++; }
                if (Global.Game.Cave[x, y + 1].Kind == CellKind.Empty) { ChangeFungus(x, y + 1, 1); delta++; }

                ChangeFungus(x, y, -delta);
            }
        }

        void ChangeFungus(int x, int y, double delta)
        {
            Cell cell = Global.Game.Cave[x, y];
            cell.Fungus += delta;
            Global.Game.Cave[x, y] = cell;
        }

        void ChangeDung(int x, int y, double delta)
        {
            Cell cell = Global.Game.Cave[x, y];
            cell.Toxic += delta;
            Global.Game.Cave[x, y] = cell;
        }

    }
}
