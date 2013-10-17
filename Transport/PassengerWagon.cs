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

    public sealed class PassengerWagon : Wagon,IComparable<PassengerWagon>
    {
        public WagonComfortClass Comfort { get; private set; }
        private int passengerCount = 0;
        private static PassengerWagonComfortComparer comfortComparer = new PassengerWagonComfortComparer();

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


        public override string ToString()
        {
            return string.Format(
                "{0}, Comfort: {1}, PassengerCount: {2}]",
                base.ToString().Substring(0, base.ToString().Length - 1), Comfort, PassengerCount);
        }

        public int CompareTo(PassengerWagon other)
        {
            return this.PassengerCount.CompareTo(other.PassengerCount);
        }

        public static PassengerWagonComfortComparer ComfortComparer
        {
            get { return comfortComparer; }
        }
    }

    public class PassengerWagonComfortComparer : IComparer<PassengerWagon>
    {
        public int Compare(PassengerWagon x, PassengerWagon y)
        {
            return ((int)x.Comfort).CompareTo((int)y.Comfort);
        }
    }
}
