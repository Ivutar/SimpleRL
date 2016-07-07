using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    class ControlManager
    {
        public ControlManager ()
        {
            //...
        }

        //Save
        //Load

        public bool MoveToLeft (ConsoleKey key)
        {
            return key == ConsoleKey.LeftArrow || key == ConsoleKey.NumPad4;
        }

        public bool MoveToRight(ConsoleKey key)
        {
            return key == ConsoleKey.RightArrow || key == ConsoleKey.NumPad6;
        }

        public bool MoveToUp(ConsoleKey key)
        {
            return key == ConsoleKey.UpArrow || key == ConsoleKey.NumPad8;
        }

        public bool MoveToDown(ConsoleKey key)
        {
            return key == ConsoleKey.DownArrow || key == ConsoleKey.NumPad2;
        }

        public bool MoveToLeftUp(ConsoleKey key)
        {
            return key == ConsoleKey.NumPad7;
        }

        public bool MoveToRightUp(ConsoleKey key)
        {
            return key == ConsoleKey.NumPad9;
        }

        public bool MoveToLeftDown(ConsoleKey key)
        {
            return key == ConsoleKey.NumPad1;
        }

        public bool MoveToRightDown(ConsoleKey key)
        {
            return key == ConsoleKey.NumPad3;
        }

    }
}
