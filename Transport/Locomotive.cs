using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public enum LocomotiveType
    {
        ElectricLoco,
        DieselLoco,
        SteamLoco
    }

    public sealed class Locomotive : RollingStock
    {
        public LocomotiveType LocomotiveType { get; private set; }
        

        public Locomotive(LocomotiveType locoType)
        {
            LocomotiveType = locoType;
        }



    }
}
