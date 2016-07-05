using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public class Menu : Widget
    {
        int current;

        public List<string> Items { get; private set; }
        public bool Horizontal { get; set; }
        public Color CurrentFore { get; set; }
        public Color CurrentBack { get; set; }
        public int Current
        {
            get
            {
                if (current < 0)
                    return 0;
                else if (current >= Items.Count)
                    return Items.Count - 1;
                else
                    return current;
            }
            set
            {
                if (value < 0)
                    current = 0;
                else if (value >= Items.Count)
                    current = Items.Count - 1;
                else
                    current = value;

                OnChange?.Invoke(this, Items[current], current);
                Draw();
            }
        }

        public Action<Menu, string, int> OnChange;
        public Action<Menu, string, int> OnSelect;

        public Menu(int width, int height, IEnumerable<string> items)
            : base(width, height)
        {
            Items = new List<string>(items);
            Fore = Color.White;
            Back = Color.Black;
            CurrentFore = Color.Black;
            CurrentBack = Color.White;
            current = 0;
        }

        public override bool Input(Event e)
        {
            if (e.Kind != EventKind.Key)
                return false;
            if (e.Kind == EventKind.Key && !e.Key.Press)
                return false;

            //here: e.Kind == EventKind.Key && e.Key.Press == true

            if (e.Key.Key == ConsoleKey.Enter)
            {
                OnSelect?.Invoke(this, Items[Current], Current);
                return true;
            }
            else
            {
                ConsoleKey dec = ConsoleKey.UpArrow;
                ConsoleKey inc = ConsoleKey.DownArrow;
                if (Horizontal)
                {
                    dec = ConsoleKey.LeftArrow;
                    inc = ConsoleKey.RightArrow;
                }

                if (e.Key.Key == dec)
                {
                    Current = Current <= 0 ? Items.Count - 1 : Current - 1;
                    OnChange?.Invoke(this, Items[Current], Current);
                }
                else if (e.Key.Key == inc)
                {
                    Current = Current >= Items.Count - 1 ? 0 : Current + 1;
                    OnChange?.Invoke(this, Items[Current], Current);
                }
                else
                    return false;

                Draw();
                return true;
            }
        }

        public override void Draw()
        {
            //clear
            Clear(new CharInfo(' ', Fore, Back));

            //items
            if (Horizontal)
            {
                //horizontal menu
                int x = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    string item = string.IsNullOrEmpty(Items[i]) ? "???" : Items[i];
                    Color fore = Fore;
                    Color back = Back;
                    if (i == Current && Focus)
                    {
                        fore = CurrentFore;
                        back = CurrentBack;
                    }
                    Write(x, 0, item, fore, back);
                    x += item.Length;
                }
            }
            else
            {
                //vertical menu
                for (int i = 0; i < Items.Count; i++)
                {
                    string item = string.IsNullOrEmpty(Items[i]) ? "???" : Items[i];
                    Color fore = Fore;
                    Color back = Back;
                    if (i == Current && Focus)
                    {
                        fore = CurrentFore;
                        back = CurrentBack;
                    }
                    Write(0, i, item, fore, back);
                }
            }
        }
    }
}
