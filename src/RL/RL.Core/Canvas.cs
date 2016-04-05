using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RL
{
    public enum Color
    {
        Black,       // 0000 0
        Blue,        // 0001 1
        Green,       // 0010 2
        Teal,        // 0011 3
        Vinous,      // 0100 4
        Violet,      // 0101 5
        Brown,       // 0110 6
        LightGray,   // 0111 7
        DarkGray,    // 1000 8
        LightBlue,   // 1001 9
        LightGreen,  // 1010 A
        Heaven,      // 1011 B
        LightRed,    // 1100 C
        LightViolet, // 1101 D
        Yellow,      // 1110 E
        White,       // 1111 F

        irgb0000 = Black,
        irgb0001 = Blue,
        irgb0010 = Green,
        irgb0011 = Teal,
        irgb0100 = Vinous,
        irgb0101 = Violet,
        irgb0110 = Brown,
        irgb0111 = LightGray,
        irgb1000 = DarkGray,
        irgb1001 = LightBlue,
        irgb1010 = LightGreen,
        irgb1011 = Heaven,
        irgb1100 = LightRed,
        irgb1101 = LightViolet,
        irgb1110 = Yellow,
        irgb1111 = White,
    }

    public struct CharInfo
    {
        public char ch;
        public Color fore;
        public Color back;

        public CharInfo(char c)
        {
            ch = c;
            fore = Color.White;
            back = Color.Black;
        }

        public CharInfo(char c, Color f, Color b)
        {
            ch = c;
            fore = f;
            back = b;
        }
    }

    public class Canvas
    {
        CharInfo[,] buffer;

        public int Width
        {
            get
            {
                if (buffer == null)
                    return 0;
                return buffer.GetLength(0);
            }
            set
            {
                int wd = value > 1 ? value : 1;
                Resize(wd, Height);
            }
        }
        public int Height
        {
            get
            {
                if (buffer == null)
                    return 0;
                return buffer.GetLength(1);
            }
            set
            {
                int hg = value > 1 ? value : 1;
                Resize(Width, hg);
            }
        }
        public CharInfo this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                    return new CharInfo();
                return buffer[x, y];
            }
            set
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                    return;
                buffer[x, y] = value;
            }
        }

        public Canvas()
        {
            buffer = new CharInfo[1, 1];
            Clear();
        }
        public Canvas(int width, int height)
        {
            width = width < 1 ? 1 : width;
            height = height < 1 ? 1 : height;
            buffer = new CharInfo[width, height];
            Clear();
        }

        public void Clear()
        {
            Clear(new CharInfo(' ', Color.White, Color.Black));
        }
        public void Clear(CharInfo ci)
        {
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                    buffer[i, j] = ci;
        }
        public void Resize(int width, int height)
        {
            width = width < 1 ? 1 : width;
            height = height < 1 ? 1 : height;

            //create the new buffer
            CharInfo[,] tmp = new CharInfo[width, height];

            //fill buffer by default values
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    tmp[i, j] = new CharInfo(' ', Color.White, Color.Black);

            //copy data from old buffer
            int wd = Math.Min(Width, width);
            int hg = Math.Min(Height, height);
            for (int j = 0; j < hg; j++)
                for (int i = 0; i < wd; i++)
                    tmp[i, j] = buffer[i, j];

            //switch to new buffer
            buffer = tmp;
        }
        public void Copy(Canvas from, int to_x = 0, int to_y = 0, int? to_width = null, int? to_height = null, int from_x = 0, int from_y = 0)
        {
            if (from == null || to_x >= Width || to_y >= Height)
                return;
            if (to_width == null) to_width = Width - to_x;
            if (to_height == null) to_height = Height - to_y;
            if (to_x < 0)
            {
                from_x -= to_x;
                to_width += to_x;
                to_x = 0;
            }
            if (to_y < 0)
            {
                from_y -= to_y;
                to_height += to_y;
                to_y = 0;
            }
            if (to_x + to_width - 1 < 0 || to_y + to_height - 1 < 0)
                return;
            if (from_x >= from.Width || from_y >= from.Height)
                return;
            if (from_x + to_width - 1 < 0 || from_y + to_height - 1 < 0)
                return;
            for (int y = to_y; y < to_y + to_height; y++)
            {
                for (int x = to_x; x < to_x + to_width; x++)
                {
                    int srcx = x - to_x + from_x;
                    int srcy = y - to_y + from_y;
                    if (srcx >= 0 && srcy >= 0 && srcx < from.Width && srcy < from.Height)
                        buffer[x, y] = from[srcx, srcy];
                }
            }
        }
        public static Canvas Load(string filename)
        {
            Canvas res = new Canvas();

            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
                return res;

            //load file
            string[] lines = File.ReadAllLines(filename);

            //size
            int width;
            int height;

            if (lines.Length <= 0)
                return res;

            string[] sizes = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (sizes.Length != 2)
                return res;

            int.TryParse(sizes[0], out width);
            int.TryParse(sizes[1], out height);

            if (width < 1) width = 1;
            if (height < 1) height = 1;

            res = new Canvas(width, height);

            //chars
            int row = 1;
            for (int y = 0; y < height && row < lines.Length; y++, row++)
            {
                string line = lines[row];
                for (int x = 0; x < width && x < line.Length; x++)
                {
                    CharInfo ci = res.buffer[x, y];
                    ci.ch = line[x];
                    res.buffer[x, y] = ci;
                }
            }

            //fore
            for (int y = 0; y < height && row < lines.Length; y++, row++)
            {
                string line = lines[row];
                for (int x = 0; x < width && x < line.Length; x++)
                {
                    CharInfo ci = res.buffer[x, y];
                    ci.fore = Char2Color(line[x]);
                    res.buffer[x, y] = ci;
                }
            }

            //back
            for (int y = 0; y < height && row < lines.Length; y++, row++)
            {
                string line = lines[row];
                for (int x = 0; x < width && x < line.Length; x++)
                {
                    CharInfo ci = res.buffer[x, y];
                    ci.back = Char2Color(line[x]);
                    res.buffer[x, y] = ci;
                }
            }

            return res;
        }
        public void Save(string filename)
        {
            List<string> lines = new List<string>();

            //size
            lines.Add(string.Format("{0} {1}", Width, Height));

            //chars
            for (int y = 0; y < Height; y++)
            {
                StringBuilder sb = new StringBuilder(Width);
                for (int x = 0; x < Width; x++)
                    sb.Append(buffer[x, y].ch);
                lines.Add(sb.ToString());
            }

            //fore
            for (int y = 0; y < Height; y++)
            {
                StringBuilder sb = new StringBuilder(Width);
                for (int x = 0; x < Width; x++)
                    sb.Append(Color2Char(buffer[x, y].fore));
                lines.Add(sb.ToString());
            }

            //back
            for (int y = 0; y < Height; y++)
            {
                StringBuilder sb = new StringBuilder(Width);
                for (int x = 0; x < Width; x++)
                    sb.Append(Color2Char(buffer[x, y].back));
                lines.Add(sb.ToString());
            }

            //save
            File.WriteAllLines(filename, lines.ToArray());
        }
        public void Write(int x, int y, string text, Color fore = Color.White, Color back = Color.Black, int? width = null, int offset = 0)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (width == null)
                width = text.Length;
            if (x >= Width || x + width - 1 < 0)
                return;
            if (y >= Height || y < 0)
                return;

            int left = x;
            int right = Math.Min(x + text.Length - 1, Width - 1);

            //indexes for 'text' argument
            int begin = left - x + offset;
            int end = right - x + offset;

            //write to buffer
            for (int i = left, j = begin; i <= right && j <= end; i++, j++)
                if (j >= 0 && j < text.Length)
                    buffer[i, y] = new CharInfo(text[j], fore, back);
                else
                    buffer[i, y] = new CharInfo(' ', fore, back);
        }

        private static Color Char2Color(char c)
        {
            c = Char.ToUpper(c);

            if (c == '0') return Color.irgb0000;
            if (c == '1') return Color.irgb0001;
            if (c == '2') return Color.irgb0010;
            if (c == '3') return Color.irgb0011;
            if (c == '4') return Color.irgb0100;
            if (c == '5') return Color.irgb0101;
            if (c == '6') return Color.irgb0110;
            if (c == '7') return Color.irgb0111;
            if (c == '8') return Color.irgb1000;
            if (c == '9') return Color.irgb1001;
            if (c == 'A') return Color.irgb1010;
            if (c == 'B') return Color.irgb1011;
            if (c == 'C') return Color.irgb1100;
            if (c == 'D') return Color.irgb1101;
            if (c == 'E') return Color.irgb1110;
            if (c == 'F') return Color.irgb1111;

            return Color.irgb0000; //by default
        }
        private static char Color2Char(Color c)
        {
            if (c == Color.irgb0000) return '0';
            if (c == Color.irgb0001) return '1';
            if (c == Color.irgb0010) return '2';
            if (c == Color.irgb0011) return '3';
            if (c == Color.irgb0100) return '4';
            if (c == Color.irgb0101) return '5';
            if (c == Color.irgb0110) return '6';
            if (c == Color.irgb0111) return '7';
            if (c == Color.irgb1000) return '8';
            if (c == Color.irgb1001) return '9';
            if (c == Color.irgb1010) return 'A';
            if (c == Color.irgb1011) return 'B';
            if (c == Color.irgb1100) return 'C';
            if (c == Color.irgb1101) return 'D';
            if (c == Color.irgb1110) return 'E';
            if (c == Color.irgb1111) return 'F';

            return '0';
        }
    }

    public static class Util
    {
        public static Canvas Buffer = new Canvas(80, 25);

        public static string Title
        {
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = value;
            }
        }
        public static int Width
        {
            get
            {
                return Console.BufferWidth;
            }
            set
            {
                Resize(value, Height);
                Buffer.Resize(Width, Height);
            }
        }
        public static int Height
        {
            get
            {
                return Console.BufferHeight;
            }
            set
            {
                Resize(Width, value);
                Buffer.Resize(Width, Height);
            }
        }
        public static bool CursorVisible
        {
            get
            {
                return Console.CursorVisible;
            }
            set
            {
                Console.CursorVisible = value;
            }
        }

        static public void Sleep(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }
        public static void Resize(int width, int height)
        {
            if (width < 1) width = 1;
            if (height < 1) height = 1;

            try { Console.WindowWidth  = width;  } catch { }
            try { Console.WindowHeight = height; } catch { }
            try { Console.BufferWidth  = width;  } catch { }
            try { Console.BufferHeight = height; } catch { }

            try { Console.WindowWidth  = width;  } catch { }
            try { Console.WindowHeight = height; } catch { }
            try { Console.BufferWidth  = width;  } catch { }
            try { Console.BufferHeight = height; } catch { }
        }
        public static void Swap()
        {
            if (Buffer == null)
                return;

            int wd = Math.Min(Util.Buffer.Width, Width);
            int hg = Math.Min(Util.Buffer.Height, Height);

            //fill buffer
            WinCon.CHAR_INFO[] buf = new WinCon.CHAR_INFO[wd * hg];
            for (int y = 0; y < hg; y++)
                for (int x = 0; x < wd; x++)
                {
                    buf[y * wd + x].UnicodeChar = Util.Buffer[x, y].ch;
                    buf[y * wd + x].Attributes =
                        (ushort)
                        (
                            (byte)((byte)(Util.Buffer[x, y].back) << 4)
                            |
                            (byte)(Util.Buffer[x, y].fore)
                        );
                }

            //set size
            WinCon.COORD size;
            size.X = (short)wd;
            size.Y = (short)hg;

            //start position
            WinCon.COORD coord;
            coord.X = 0;
            coord.Y = 0;

            //target rect
            WinCon.SMALL_RECT rc;
            rc.Left = (short)0;
            rc.Right = (short)(wd - 1);
            rc.Top = (short)0;
            rc.Bottom = (short)(hg - 1);

            //write to console
            IntPtr handle = WinCon.GetStdHandle((int)-11);
            WinCon.WriteConsoleOutput(handle, buf, size, coord, ref rc);
        }

        #region [ Read from config ]

        //read from config (bool)
        public static bool ReadConfigBool(string key, bool dflt = false)
        {
            bool val = dflt;

            string str = ConfigurationManager.AppSettings[key];
            if (str != null)
            {
                str = str.Trim().ToLower();
                if (str == "1" || str == "true" || str == "y" || str == "yes") val = true;
                if (str == "0" || str == "false" || str == "n" || str == "no" || str == "not" || str == "none") val = false;
            }

            return val;
        }

        //read from config (string)
        public static string ReadConfigString(string key, string dflt = "")
        {
            string val = dflt;

            string str = ConfigurationManager.AppSettings[key];
            if (str != null)
                val = str;

            return val;
        }

        //read from config (char)
        public static char ReadConfigChar(string key, char dflt = ' ')
        {
            char val = dflt;

            string str = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(str))
                val = str[0];

            return val;
        }

        //read from config (int)
        public static int ReadConfigInt(string key, int dflt = 0, int min = int.MinValue, int max = int.MaxValue)
        {
            if (String.IsNullOrEmpty(key))
                return dflt;

            int val = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings[key], out val))
                val = dflt;

            //foolproof
            if (min > max) { int t = min; min = max; max = min; }

            //limits
            if (val < min) val = min;
            if (val > max) val = max;

            return val;
        }

        //read from config (Color)
        public static Color ReadConfigColor(string key, Color dflt = Color.Black)
        {
            if (String.IsNullOrEmpty(key))
                return dflt;

            Color val = dflt;
            if (!Enum.TryParse<Color>(ConfigurationManager.AppSettings[key], out val))
                val = dflt;

            return val;
        }

        #endregion

    }
}
