using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL.FOV
{
    public interface IFOV
    {
        double ViewRadius { get; set; }

        Func<int, int, bool> IsSolidBlock { get; set; }

        Action<int, int, double> SetBlockLightDistanceSquared { get; set; }

        void ComputeVisibility(int posx, int posy);
    }
}
