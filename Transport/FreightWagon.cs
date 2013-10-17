using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public enum CargoType
    {
        Wood,
        Coal,
        Animals,
        Transport,
        Luggage
    }

    public sealed class FreightWagon : Wagon
    {
        public CargoType Cargo { get; private set; }
        private int load;
        
        public FreightWagon(CargoType cargo, int capacity)
            : base(capacity)
        {
            Cargo = cargo;
        }
        public FreightWagon(CargoType cargo)
        {
            Cargo = cargo;
        }
        public FreightWagon()
            : this(CargoType.Coal)
        {

        }


        public int Load
        {
            get { return load; }
            set
            {
                if (Capacity >= value)
                    load = value;
                else throw new Exception("Wagon is full");
            }
        }


        public void LoadWagon(int volume)
        {
            Load += volume;
        }

        public void UnloadWagon()
        {
            Load = 0;
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
                "{0}, CargoType: {1}, Load: {2}]",
                base.ToString().Substring(0, base.ToString().Length - 1), Cargo, Load);
        }
    }
}
