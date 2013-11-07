using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Telephone:IConnectable
    {
        int id;
        long telephoneNumber;
        Port port;

        public void Disconnect(IHaveConnection device)
        {
            throw new NotImplementedException();
        }

        public void Connect(IHaveConnection device)
        {
            Port dev = device as Port;
            if (dev == null) throw new ArgumentException("You cannot connect this device");

            if (!Connected && !dev.Connected)
            {
                port = dev;
                port.Connect(this);
            }
        }


        public bool Connected
        {
            get { return port != null; }
        }
    }
}
