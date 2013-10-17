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
        private bool[] sits;
        private static PassengerWagonComfortComparer comfortComparer = new PassengerWagonComfortComparer();

        public PassengerWagon()
            : this(WagonComfortClass.Middle)
        {
        }

        public PassengerWagon(WagonComfortClass comfort)
        {
            Comfort = comfort;
            sits = new bool[Capacity];
        }

        public PassengerWagon(WagonComfortClass comfort, int capacity)
            : base(capacity)
        {
            Comfort = comfort;
            sits = new bool[capacity];
        }

        public bool this[int place]
        {
            get 
            {
                if (place < 0 || place >= Capacity)
                    throw new IndexOutOfRangeException("Index is not valid");
                return sits[place];
            }
            set 
            {
                if (place < 0 || place >= Capacity)
                    throw new IndexOutOfRangeException("Index is not valid");
                sits[place] = value;
            }
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
            sits[PassengerCount++] = true;            
        }
        public void RemovePassengers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                sits[--PassengerCount] = false;
            }
        }
        public void RemovePassengers()
        {
            while (--PassengerCount >= 0) 
            {
                sits[PassengerCount] = false;
            }
        }
        /// <summary>
        /// Returns copy of array which contains info about sit places
        /// </summary>
        /// <returns></returns>
        public bool[] GetSits()
        {
            bool[] temp = new bool[Capacity];
            Array.Copy(sits, temp, Capacity);
            return temp;
        }

        public override void Close()
        {
            Console.WriteLine("{0}.Close()", GetType().FullName);
        }
        public override void Open()
        {
            Console.WriteLine("{0}.Open()", GetType().FullName);
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
