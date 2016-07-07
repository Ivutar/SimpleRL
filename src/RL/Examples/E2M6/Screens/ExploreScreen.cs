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
                if (e.Key.Key == ConsoleKey.Escape) Global.CurrentScreen = Global.MainMenu;
                else if (Global.Control.MoveToLeft     (e.Key.Key)) Move(-1,  0);
                else if (Global.Control.MoveToRight    (e.Key.Key)) Move(+1,  0);
                else if (Global.Control.MoveToUp       (e.Key.Key)) Move( 0, -1);
                else if (Global.Control.MoveToDown     (e.Key.Key)) Move( 0, +1);
                else if (Global.Control.MoveToLeftUp   (e.Key.Key)) Move(-1, -1);
                else if (Global.Control.MoveToRightUp  (e.Key.Key)) Move(+1, -1);
                else if (Global.Control.MoveToLeftDown (e.Key.Key)) Move(-1, +1);
                else if (Global.Control.MoveToRightDown(e.Key.Key)) Move(+1, +1);
            }
        }

        public override void Draw()
        {
            Util.Buffer.Clear();
            
            //draw cave
            for (int x = 0; x<Util.Buffer.Width; x++)
                for (int y = 0; y < Util.Buffer.Height; y++)
                {
                    Cell cell = Global.Cave[x + viewx, y + viewy];
                    Color f = Color.Black;
                    Color b = Color.Black;
                    string ch = " ";

                    if (cell.Kind == CellKind.Empty)     ch = ".";
                    else if (cell.Kind == CellKind.Wall) ch = "#";

                    if (!cell.Explored) f = Color.Black;        //unexplored
                    else if (cell.Visible) f = Color.LightGray; //visible
                    else f = Color.DarkGray;                    //explored but not visible

                    Util.Buffer.Write(x, y, ch, f, b);
                }

            //draw monsters, items and etc
            //...

            //draw hero
            Util.Buffer.Write(Global.Hero.X - viewx, Global.Hero.Y - viewy, "@", Color.LightRed, Color.Black);

            Util.Swap();
        }

        void Move (int dx, int dy)
        {
            Global.MoveHero(dx, dy);
            UpdateView();
        }

        public void UpdateView()
        {
            viewx = FitView(Global.Hero.X, Util.Buffer.Width, Global.Cave.Width);
            viewy = FitView(Global.Hero.Y, Util.Buffer.Height, Global.Cave.Height);
        }

        int FitView (int p, int s, int m)
        {
            if (p < s / 2) return 0;
            if (p >= m - (s / 2)) return m - s;
            return p - (s / 2);
        }

    }
}
