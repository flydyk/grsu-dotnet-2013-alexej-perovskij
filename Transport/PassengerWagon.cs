using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public enum WagonComfortClass
    {
        Cheap,
        Middle,
        Business
    }

    public sealed class PassengerWagon : Wagon
    {
        public WagonComfortClass Comfort { get; private set; }

        public PassengerWagon()
            : this(WagonComfortClass.Middle)
        {
        }

        public PassengerWagon(WagonComfortClass comfort)
            : base()
        {
            Comfort = comfort;
        }

        public PassengerWagon(WagonComfortClass comfort, int capacity)
            : base(capacity)
        {
            Comfort = comfort;
        }
    }
}
