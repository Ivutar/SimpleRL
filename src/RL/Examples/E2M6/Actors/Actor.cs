using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    struct UpdateEvent
    {
        public double EnegryCost;
        public bool StopUpdating;
    }

    abstract class Actor
    {
        public double Energy { get; set; } //0-1000
        public double Speed { get; set; } //~100

        public abstract void Update(ref UpdateEvent e);
    }
}
