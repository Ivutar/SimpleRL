using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6
{
    abstract class LiveActor : Actor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
    }
}
