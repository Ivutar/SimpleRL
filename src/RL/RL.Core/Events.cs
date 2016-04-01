using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public enum EventKind
    {
        Empty = 0, //empty event
        Key,       //keyboard event
        Mouse,     //mouse event
    }

    [Flags]
    public enum Modifiers
    {
        RIGHT_ALT_PRESSED  = 0x0001,
        LEFT_ALT_PRESSED   = 0x0002,
        RIGHT_CTRL_PRESSED = 0x0004,
        LEFT_CTRL_PRESSED  = 0x0008,
        SHIFT_PRESSED      = 0x0010,
        NUMLOCK_ON         = 0x0020,
        SCROLLLOCK_ON      = 0x0040,
        CAPSLOCK_ON        = 0x0080,
        ENHANCED_KEY       = 0x0100,
    }

    public enum MouseEventKind
    {
        Move,
        Press,
        Release,
    }

    public struct KeyEvent
    {
        public ConsoleKey Key;
        public char       KeyChar;
        
        public bool Press;
        public bool Alt;
        public bool Shift;
        public bool Ctrl;
        public bool Auto;

        public override string ToString()
        {
            return string.Format(
                "{0} {1} {2} {3} {4} {5} {6}",
                Key.ToString("F"),
                char.IsControl(KeyChar) ? "" : "'" + KeyChar + "'",
                Press ? "Press" : "Release",
                Alt ? "Alt" : "",
                Shift ? "Shift" : "",
                Ctrl ? "Ctrl" : "",
                Auto ? "Auto" : ""
            );
        }
    }

    public struct MouseEvent
    {
        public MouseEventKind Kind;
        public bool Left;
        public bool Right;
        public int X;
        public int Y;

        public override string ToString()
        {
            return string.Format(
                "{0} ({1}, {2}) {3} {4}",
                Kind.ToString("F"),
                X, Y,
                Left ? "Left" : "",
                Right ? "Right" : ""
            );
        }
    }

    public struct Event
    {
        static public Event EmptyEvent;

        public EventKind Kind;

        public KeyEvent   Key;
        public MouseEvent Mouse;

        public bool Empty
        {
            get { return Kind == EventKind.Empty; }
        }

        public Event(EventKind Kind, KeyEvent Key = new KeyEvent(), MouseEvent Mouse = new MouseEvent())
        {
            this.Kind  = Kind;
            this.Key   = Key;
            this.Mouse = Mouse;
        }

        public override string ToString()
        {
            switch (Kind)
            {
                case EventKind.Empty:
                    return "Empty";
                case EventKind.Key:
                    return string.Format("Key: {0}", Key);
                case EventKind.Mouse:
                    return string.Format("Mouse: {0}", Mouse);
            }
            return "";
        }
    }

    public static class Events
    {
        const int  STD_INPUT_HANDLE       = -10;
        const int  KEY_EVENT              = 0x0001;
        const int  MOUSE_EVENT            = 0x0002;
        const int  ENABLE_MOUSE_INPUT     = 0x0010;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const int  ENABLE_EXTENDED_FLAGS  = 0x0080;

        static List<Event> list = new List<Event>(100);
        static bool LeftButton = false;
        static bool RightButton = false;

        static Events ()
        {
            var handle = WinCon.GetStdHandle(STD_INPUT_HANDLE);
            uint mode = 0;
            if (!(WinCon.GetConsoleMode(handle, out mode))) { throw new System.ComponentModel.Win32Exception(); }
            mode |= ENABLE_MOUSE_INPUT;
            mode &= ~ENABLE_QUICK_EDIT_MODE;
            mode |= ENABLE_EXTENDED_FLAGS;
            if (!(WinCon.SetConsoleMode(handle, mode))) { throw new System.ComponentModel.Win32Exception(); }
        }

        public static bool HasEvent
        {
            get
            {
                ReadBufferEvents();
                return list.Count > 0;
            }
        }

        public static Event GetNext(bool wait = false)
        {
            ReadBufferEvents();

            if (wait)
            {
                while (!HasEvent)
                    System.Threading.Thread.Sleep(10);
            }
            else
            {
                if (!HasEvent)
                    return Event.EmptyEvent;
            }

            Event e = list[0];
            list.RemoveAt(0);
            return e;
        }

        static void ReadBufferEvents()
        {
            IntPtr handle = WinCon.GetStdHandle(STD_INPUT_HANDLE);
            WinCon.INPUT_RECORD[] buff = new WinCon.INPUT_RECORD[1];
            uint count;

            //read all mouse and keyboard events
            while (true)
            {
                //exit if have no events in buffer
                WinCon.GetNumberOfConsoleInputEvents(handle, out count);
                if (count <= 0)
                    break;

                //get next event from buffer
                WinCon.ReadConsoleInput(handle, buff, (uint)buff.Length, out count);
                if (count <= 0)
                    break;
                else
                {
                    WinCon.INPUT_RECORD ir = buff[0];

                    //key
                    if (ir.EventType == KEY_EVENT)
                    {
                        Event e = new Event();

                        e.Kind = EventKind.Key;

                        e.Key.Key = (ConsoleKey)ir.KeyEvent.wVirtualKeyCode;
                        e.Key.KeyChar = ir.KeyEvent.UnicodeChar;
                        e.Key.Press = ir.KeyEvent.bKeyDown;

                        Modifiers m = (Modifiers)ir.KeyEvent.dwControlKeyState;

                        e.Key.Alt = m.HasFlag(Modifiers.LEFT_ALT_PRESSED) || m.HasFlag(Modifiers.RIGHT_ALT_PRESSED);
                        e.Key.Shift = m.HasFlag(Modifiers.SHIFT_PRESSED);
                        e.Key.Ctrl = m.HasFlag(Modifiers.LEFT_CTRL_PRESSED) || m.HasFlag(Modifiers.RIGHT_CTRL_PRESSED);
                        e.Key.Auto = false; //ir.KeyEvent.wRepeatCount; //... auto key press (probably not work)?

                        list.Add(e);
                    }

                    //mouse
                    if (ir.EventType == MOUSE_EVENT && ir.MouseEvent.dwEventFlags == 0 || ir.MouseEvent.dwEventFlags == 1)
                    {
                        //move
                        if (ir.MouseEvent.dwEventFlags == 1)
                        {
                            LeftButton = (ir.MouseEvent.dwButtonState & 1) != 0;
                            RightButton = (ir.MouseEvent.dwButtonState & 2) != 0;

                            Event e = new Event();

                            e.Kind = EventKind.Mouse;

                            e.Mouse.Kind = MouseEventKind.Move;
                            e.Mouse.X = ir.MouseEvent.dwMousePosition.X;
                            e.Mouse.Y = ir.MouseEvent.dwMousePosition.Y;
                            e.Mouse.Left = LeftButton;
                            e.Mouse.Right = RightButton;

                            list.Add(e);
                        }

                        //press/release
                        if (ir.MouseEvent.dwEventFlags == 0)
                        {
                            Event e = new Event();

                            e.Kind = EventKind.Mouse;

                            e.Mouse.X = ir.MouseEvent.dwMousePosition.X;
                            e.Mouse.Y = ir.MouseEvent.dwMousePosition.Y;

                            //mouse button event (press or release)
                            e.Mouse.Kind = MouseEventKind.Release;
                            bool left = (ir.MouseEvent.dwButtonState & 1) != 0;
                            bool right = (ir.MouseEvent.dwButtonState & 2) != 0;
                            if (!LeftButton && left) e.Mouse.Kind = MouseEventKind.Press;
                            if (!RightButton && right) e.Mouse.Kind = MouseEventKind.Press;

                            LeftButton = left;
                            RightButton = right;

                            e.Mouse.Left = LeftButton;
                            e.Mouse.Right = RightButton;

                            list.Add(e);
                        }
                    }
                }
            }
        }
    }
}
