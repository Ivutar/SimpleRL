using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E4M1
{
    public class Viewer : Panel
    {
        int startx = 0;
        int starty = 0;
        bool move = false;

        public int DX { get; set; }
        public int DY { get; set; }
        public Canvas Content { get; set; }

        public Viewer(int width, int height): base(width, height)
        {
            DX = 0;
            DY = 0;
        }

        public override bool Input(Event e)
        {
            if (e.Kind == EventKind.Mouse)
            {
                if (e.Mouse.Kind == MouseEventKind.Press && e.Mouse.Left)
                {
                    startx = e.Mouse.X;
                    starty = e.Mouse.Y;
                    move = true;
                }
                else if (e.Mouse.Kind == MouseEventKind.Release && !e.Mouse.Left)
                {
                    move = false;
                }
                else if (move && e.Mouse.Kind == MouseEventKind.Move)
                {
                    DX += startx - e.Mouse.X;
                    DY += starty - e.Mouse.Y;
                    startx = e.Mouse.X;
                    starty = e.Mouse.Y;
                    Draw();
                }
            }
            return false;
        }

        public override void Draw()
        {
            Clear();

            //content
            Copy(Content, 0, 0, Width, Height, DX, DY);

            //rullers
            Color f = Color.Black;
            Color b = Color.White;
            Write(0, 0, "▒", f, b);
            Write(Width - 1, 0, "▒", f, b);
            Write(0, Height - 1, "▒", f, b);
            Write(Width - 1, Height - 1, "▒", f, b);
            for (int x = 1; x < Width-1; x++)
            {
                int i = x + DX;
                if (i % 8 == 0)
                {
                    Write(x, 0, "┼", f, b);
                    Write(x, Height - 1, "┼", f, b);
                }
                else if (i % 4 == 0)
                {
                    Write(x, 0, "┬", f, b);
                    Write(x, Height - 1, "┴", f, b);
                }
                else
                {
                    Write(x, 0, "─", f, b);
                    Write(x, Height - 1, "─", f, b);
                }
            }
            for (int y = 1; y < Height - 1; y++)
            {
                int i = y + DY;
                if (i % 8 == 0)
                {
                    Write(0, y, "┼", f, b);
                    Write(Width - 1, y, "┼", f, b);
                }
                else if (i % 4 == 0)
                {
                    Write(0, y, "├", f, b);
                    Write(Width - 1, y, "┤", f, b);
                }
                else
                {
                    Write(0, y, "│", f, b);
                    Write(Width - 1, y, "│", f, b);
                }
            }
        }
    }
}
