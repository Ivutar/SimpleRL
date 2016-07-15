using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6.Screens
{
    class ExploreScreen : Screen
    {
        int viewx = 0;
        int viewy = 0;

        public ExploreScreen ()
        {
        }

        public override void Input(Event e)
        {
            if (e.Kind == EventKind.Key && e.Key.Press)
            {
                if (e.Key.Key == ConsoleKey.Escape) Global.Sys.CurrentScreen = Global.Sys.MainMenu;
                else if (Global.Sys.Control.Wait           (e.Key.Key)) Move( 0,  0);
                else if (Global.Sys.Control.MoveToLeft     (e.Key.Key)) Move(-1,  0);
                else if (Global.Sys.Control.MoveToRight    (e.Key.Key)) Move(+1,  0);
                else if (Global.Sys.Control.MoveToUp       (e.Key.Key)) Move( 0, -1);
                else if (Global.Sys.Control.MoveToDown     (e.Key.Key)) Move( 0, +1);
                else if (Global.Sys.Control.MoveToLeftUp   (e.Key.Key)) Move(-1, -1);
                else if (Global.Sys.Control.MoveToRightUp  (e.Key.Key)) Move(+1, -1);
                else if (Global.Sys.Control.MoveToLeftDown (e.Key.Key)) Move(-1, +1);
                else if (Global.Sys.Control.MoveToRightDown(e.Key.Key)) Move(+1, +1);
            }
        }

        public override void Draw()
        {
            Util.Buffer.Clear();
            
            //draw cave
            CharInfo[] ch = new CharInfo[1];
            for (int x = 0; x<Util.Buffer.Width; x++)
                for (int y = 0; y < Util.Buffer.Height; y++)
                {
                    ch[0] = GetCellCharInfo(x + viewx, y + viewy);
                    Util.Buffer.Write(x, y, ch);
                }

            //draw monsters, items and etc
            //...

            //draw hero
            Util.Buffer.Write(Global.Game.Hero.X - viewx, Global.Game.Hero.Y - viewy, "@", Color.LightRed, Color.Black);

            Util.Swap();
        }

        public void UpdateView()
        {
            viewx = FitView(Global.Game.Hero.X, Util.Buffer.Width, Global.Game.Cave.Width);
            viewy = FitView(Global.Game.Hero.Y, Util.Buffer.Height, Global.Game.Cave.Height);
        }

        CharInfo GetCellCharInfo(int x, int y)
        {
            CharInfo res = new CharInfo(); //default
            Cell cell = Global.Game.Cave[x,y];

            if (!Global.Cfg.ShowAll && !cell.Visible)
            {
                //unexplored or memorized
                res = cell.Memorized;
                res.fore = Color.DarkGray;
                return res;
            }
            else
            {
                //base cell
                if (cell.Kind == CellKind.Wall)       res = new CharInfo('#', Color.LightGray, Color.Black);
                else if (cell.Kind == CellKind.Empty) res = new CharInfo('.', Color.LightGray, Color.Black);

                //fungus
                if (cell.Fungus > 0) res = new CharInfo('\'', Color.Green, Color.Black);
                if (cell.Fungus > Global.Cfg.FungusYoung) res = new CharInfo('"', Color.Green, Color.Black);
                if (cell.Fungus > Global.Cfg.FungusMid) res = new CharInfo('"', Color.Yellow, Color.Black);

                //items
                //...

                //npc
                //...

                //memorize
                if (!Global.Cfg.ShowAll)
                {
                    cell.Memorized = res;
                    Global.Game.Cave[x, y] = cell;
                }
            }

            return res;
        }

        void Move (int dx, int dy)
        {
            Global.Game.MoveHero(dx, dy);
            UpdateView();
        }

        int FitView (int p, int s, int m)
        {
            if (p < s / 2) return 0;
            if (p >= m - (s / 2)) return m - s;
            return p - (s / 2);
        }

    }
}
