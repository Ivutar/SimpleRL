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
        int fungusbirth;
        double fungusspeed;
        double fungusyoung;
        double fungusmid;
        double fungusold;

        public bool ShowAll { get { return showall; } }

        public int FungusBirth { get { return fungusbirth; } }
        public double FungusSpeed { get { return fungusspeed; } }
        public double FungusYoung { get { return fungusyoung; } }
        public double FungusMid { get { return fungusmid; } }
        public double FungusOld { get { return fungusold; } }

        public CfgData ()
        {
            showall = Util.ReadConfigBool("DBG.ShowAll");

            fungusbirth = Util.ReadConfigInt   ("FUNGUS.BIRTH", 5, 0, 100);
            fungusspeed = Util.ReadConfigDouble("FUNGUS.SPEED", 50);
            fungusyoung = Util.ReadConfigDouble("FUNGUS.YOUNG", 3);
            fungusmid   = Util.ReadConfigDouble("FUNGUS.MID",   7);
            fungusold   = Util.ReadConfigDouble("FUNGUS.OLD",   10);
        }
    }
}
