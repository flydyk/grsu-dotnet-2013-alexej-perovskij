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
        private int passengerCount = 0;

        public int PassengerCount
        {
            get { return passengerCount; }
            private set 
            {
                if (value <= 0) passengerCount = 0;
                else
                if (Capacity >= value)
                    passengerCount = value;
                else throw new Exception("Wagon is full");
            }
        }
        public void AddPassenger()
        {
            PassengerCount++;
        }
        public void RemovePassenger(int count)
        {
            PassengerCount -= count;
        }
        public void RemovePassenger()
        {
            PassengerCount = 0;
        }

        public PassengerWagon()
            : this(WagonComfortClass.Middle)
        {
        }

        public PassengerWagon(WagonComfortClass comfort)
        {
            Comfort = comfort;
        }

        public PassengerWagon(WagonComfortClass comfort, int capacity)
            : base(capacity)
        {
            Comfort = comfort;
        }

        public override string ToString()
        {
            return string.Format(
                "{0}, Comfort: {1}, PassengerCount: {2}]",
                base.ToString().Substring(0, base.ToString().Length - 1), Comfort, PassengerCount);
        }
    }
}
