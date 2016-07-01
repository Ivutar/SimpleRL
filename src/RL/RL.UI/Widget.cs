using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public class Widget : Canvas
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public Color Fore { get; set; }
        public Color Back { get; set; }

        public bool Visible { get; set; }
        public bool Focus { get; set; }

        /// <summary>
        /// Performs event processing and redraws widget if it is required
        /// </summary>
        /// <param name="e">event for widget processing</param>
        /// <returns>returns true - if event was processed and widget was redrawn</returns>
        public virtual bool Input(Event e) { return false; }

        public virtual void Draw() { }

        public Widget() { Visible = true; }
        public Widget(int width, int height) : base(width, height) { Visible = true; }
    }
}
