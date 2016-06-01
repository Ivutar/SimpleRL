using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL.FOV
{
    interface IFOV
    {
        double ViewRadius { get; set; }
        void ComputeVisibility(int posx, int posy, Func<int, int, bool> isSolidBlock, Action<int, int, double> setBlockLightDistanceSquared);
    }
}
