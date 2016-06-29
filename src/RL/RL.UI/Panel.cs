using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL
{
    public class Panel : Widget
    {
        Widget focuson = null;

        public List<Widget> Widgets = new List<Widget>();
        public Canvas Target { get; set; }
        public Widget FocusOn {
            get
            {
                return focuson;
            }
            set
            {
                if (focuson != null)
                    focuson.Focus = false;
                focuson = value;
                if (focuson != null)
                    focuson.Focus = true;
                Draw();
            }
        }

        public Widget Prev(Widget widget)
        {
            int i = Widgets.IndexOf(widget);
            if (i == -1)
                return null;

            i = i - 1 < 0  ? Widgets.Count - 1 : i - 1;
            return Widgets[i];
        }

        public Widget Next(Widget widget)
        {
            int i = Widgets.IndexOf(widget);
            if (i == -1)
                return null;

            i = i + 1 >= Widgets.Count ? 0 : i + 1;
            return Widgets[i];
        }

        public Panel() : base () { }
        public Panel(int width, int height) : base(width, height) { }
        public Panel(int width, int height, Canvas target) : base(width, height) { Target = target; }

        /// <summary>
        /// returns true if event propagation must be stopped
        /// </summary>
        public Func<Panel, Widget, Event, bool> BeforeInput;

        public override bool Input(Event e)
        {
            if (BeforeInput != null)
                if (BeforeInput(this, FocusOn, e))
                {
                    Draw();
                    return true;
                }

            if (FocusOn != null)
                if (FocusOn.Input(e))
                {
                    Draw();
                    return true;
                }

            return false;
        }

        public override void Draw()
        {
            Clear(new CharInfo(' ', Fore, Back));

            foreach (var widget in Widgets)
                if (widget.Visible)
                {
                    widget.Draw();
                    Copy(widget, widget.Left, widget.Top);
                }

            if (Target != null)
                Target.Copy(this, Left, Top);
        }
    }
}
