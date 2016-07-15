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
                {
                    if (Cave[i, j].Fungus > 0)
                    {
                        //fungus grow
                        if (Cave[i, j].Fungus < Global.Cfg.FungusOld)
                            ChangeFungus(i, j, 1);

                        if (Cave[i, j].Fungus > Global.Cfg.FungusYoung)
                        {
                            //expand fungus
                            ExpandFungus(i, j);

                            //inc toxic
                            ChangeToxic(i, j, 1);
                        }

                        //die fungus
                        if (Cave[i, j].Fungus < Cave[i, j].Toxic)
                            DieFungus(i, j);
                    }
                    else if (Cave[i, j].Toxic > 0)
                    {
                        //dec toxic
                        ChangeToxic(i,j, -1);

                        //spores
                        if (Cave[i, j].Toxic <= 0 && Cave[i, j].Spores)
                            if (Global.rnd.Next(100) <= Global.Cfg.FungusBirth)
                                ChangeFungus(i, j, 1);
                    }
                }
                    
        }

        void ExpandFungus(int x, int y)
        {
            if (Global.Game.Cave[x, y].Fungus > Global.Cfg.FungusYoung)
            {
                if (Global.Game.Cave[x - 1, y].Kind == CellKind.Empty && Global.Game.Cave[x - 1, y].Toxic <= 0) ChangeFungus(x - 1, y, 1); 
                if (Global.Game.Cave[x + 1, y].Kind == CellKind.Empty && Global.Game.Cave[x + 1, y].Toxic <= 0) ChangeFungus(x + 1, y, 1);
                if (Global.Game.Cave[x, y - 1].Kind == CellKind.Empty && Global.Game.Cave[x, y - 1].Toxic <= 0) ChangeFungus(x, y - 1, 1);
                if (Global.Game.Cave[x, y + 1].Kind == CellKind.Empty && Global.Game.Cave[x, y + 1].Toxic <= 0) ChangeFungus(x, y + 1, 1);
            }
        }

        void DieFungus(int x, int y)
        {
            Cell cell = Global.Game.Cave[x, y];
            cell.Fungus = 0;
            cell.Spores = true;
            Global.Game.Cave[x, y] = cell;
        }

        void ChangeFungus(int x, int y, double delta)
        {
            Cell cell = Global.Game.Cave[x, y];
            cell.Fungus += delta;
            Global.Game.Cave[x, y] = cell;
        }

        void ChangeToxic(int x, int y, double toxic)
        {
            Cell cell = Global.Game.Cave[x, y];
            cell.Toxic += toxic;
            Global.Game.Cave[x, y] = cell;
        }

    }
}
