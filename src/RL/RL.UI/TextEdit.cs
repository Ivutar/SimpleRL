using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public class TextEdit : Widget
    {
        public string Text
        {
            get { return text; }
            set
            { 
                if (value != null)
                    text = value;

                if (MaxLength != null && text.Length >= MaxLength)
                {
                    int max = MaxLength ?? 0;
                    text = text.Substring(0, max);
                    if (position >= max)
                        position = max - 1;
                }

                UpdateOffset();
            }
        }
        public Color TextFore { get; set; }
        public Color TextBack { get; set; }
        public Color CursorFore { get; set; }
        public Color CursorBack { get; set; }
        public int? MaxLength { get { return maxlength; } set { if (value == null || value > 0) { maxlength = value; Text = text; } } }

        string text;
        int? maxlength;
        int position;
        int offset;

        public TextEdit(int width)
            : base(width, 1)
        {
            text = "";

            TextFore   = Color.LightGray; 
            TextBack   = Color.Black;
            CursorFore = Color.Black;
            CursorBack = Color.LightGray;

            maxlength = null;
            position = 0;
            offset = 0;
        }

        public override bool ProcessEvent(Event e)
        {
            if (e.Kind != EventKind.Key)
                return false;
            if (e.Kind == EventKind.Key && !e.Key.Press)
                return false;

            //here: e.Kind == EventKind.Key && e.Key.Press == true

            if (e.Key.Key == ConsoleKey.LeftArrow)
            {
                //left
                if (position == 0)
                    return false;

                position--;
                UpdateOffset();
            }
            else if (e.Key.Key == ConsoleKey.RightArrow)
            {
                //right
                if (position >= Text.Length)
                    return false;
                position++;
                UpdateOffset();
            }
            else if (e.Key.Key == ConsoleKey.Home)
            {
                //home
                position = 0;
                UpdateOffset();
            }
            else if (e.Key.Key == ConsoleKey.End)
            {
                //end
                position = Text.Length;
                UpdateOffset();
            }
            else if (e.Key.Key == ConsoleKey.Delete)
            {
                //delete
                string left = Text.Substring(0, position);
                string right = "";
                if (Text.Length >= position + 1)
                    right = Text.Substring(position + 1);
                Text = left + right;
            }
            else if (e.Key.Key == ConsoleKey.Backspace)
            {
                //backspace
                if (position == 0)
                    return false;

                position--;

                string left = Text.Substring(0, position);
                string right = "";
                if (Text.Length >= position + 1)
                    right = Text.Substring(position + 1);
                Text = left + right;

                UpdateOffset();
            }
            else if (!char.IsControl(e.Key.KeyChar))
            {
                //insert char
                if (MaxLength != null && Text.Length >= MaxLength)
                {
                    int max = MaxLength ?? 0;
                    Text = Text.Substring(0, max - 1);
                    if (position >= max)
                        position = max - 1;
                }

                string left = Text.Substring(0, position);
                string right = Text.Substring(position);
                Text = left + e.Key.KeyChar + right;
                position++;

                if (MaxLength != null && position >= (MaxLength ?? 0))
                    position = (MaxLength ?? 0) - 1;

                UpdateOffset();
            }
            else
            {
                //control char
                return false;
            }

            Draw();
            return true;
        }

        public override void Draw()
        {
            Clear(new CharInfo(' ', TextFore, TextBack));
            Write(0, 0, Text, TextFore, TextBack, null, offset);
            if (Focus)
            {
                string cursor = " ";
                if (position < Text.Length)
                    cursor = Text[position].ToString();
                Write(position - offset, 0, cursor, CursorFore, CursorBack);
            }
        }

        void UpdateOffset()
        {
            if (position - offset < 0)
                offset = position;
            else if (position - offset >= Width)
                offset = position - Width + 1;
        }
    }
}
