using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Port:IHaveConnection
    {
        int id;
        
        public bool IsBusy { get; set; }
        IHaveConnection dev = null;

        public Port(int id)
        {
            ID = id;
        }
        public bool Connected 
        {
            get { return dev != null; }
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


        public void Connect(IHaveConnection device)
        {
            if (!device.Connected && !Connected)
                dev = device;
        }
    }
}
