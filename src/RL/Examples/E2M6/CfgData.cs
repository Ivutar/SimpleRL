using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RL;

namespace E2M6
{
    class CfgData
    {
        bool showall;

        public bool ShowAll { get { return showall; } }

        public CfgData ()
        {
            showall = Util.ReadConfigBool("DBG.ShowAll");
        }
    }
}
