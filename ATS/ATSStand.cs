using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class ATSStand:IEnumerable<Port>
    {
        int id;
        Dictionary<int,Port> ports;
        public const int PORTS_COUNT = 20;

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
                if (value >= 0)
                    id = value;
                else throw new ArgumentOutOfRangeException("ID value must be greater than zero");
            }
        }
        /// <summary>
        /// Get Port by ID
        /// </summary>
        /// <param name="id">ID of the port</param>
        /// <returns>Port of the ATSStand</returns>
        public Port this[int id]
        {
            get { return ports[id]; }
        }

        public IEnumerator<Port> GetEnumerator()
        {
            foreach (int item in ports.Keys)
            {
                yield return ports[item];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ports.Values.GetEnumerator();
        }
    }
}
