using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class ATSStand
    {
        int id;
        Dictionary<int,Port> ports;
        const int PORTS_COUNT = 20;

        public ATSStand(int id)
        {
            ID = id;
            ports = new Dictionary<int, Port>(PORTS_COUNT);
            FillStand();
        }

        private void FillStand()
        {
            for (int i = 0; i < PORTS_COUNT; i++)
            {
                ports.Add(i, new Port(i));
            }
        }


        public int ID
        {
            get { return id; }
            private set
            {
                if (value > 0)
                    id = value;
                else throw new ArgumentOutOfRangeException("ID value must be greater than zero");
            }
        }

        public Port this[int id]
        {
            get { return ports[id]; }
        }
    }
}
