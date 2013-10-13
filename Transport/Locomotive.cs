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
        readonly int maxSpeed = 150;
        readonly int power = 5000;
        
        public Locomotive(LocomotiveType locoType)
        {
            LocomotiveType = locoType;
            switch (LocomotiveType)
            {
                case Transport.LocomotiveType.DieselLoco:
                    maxSpeed = 150;
                    power = 5000;
                    break;
                case Transport.LocomotiveType.ElectricLoco:
                    maxSpeed = 250;
                    power = 8000;
                    break;
                case Transport.LocomotiveType.SteamLoco:
                    maxSpeed = 80;
                    power = 2000;
                    break;
                default: 
                    maxSpeed = 60;
                    power = 1000;
                    break;
            }
        }

        public Locomotive()
            : this(LocomotiveType.DieselLoco)
        { }

        public int Power
        {
            get { return power; }
        }
        public int MaxSpeed
        {
            get { return maxSpeed; }
        }


        public void Run() { }
        public void Stop() { }

    }
}
