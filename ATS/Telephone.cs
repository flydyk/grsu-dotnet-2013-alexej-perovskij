using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Telephone : IConnectable,ICanCall
    {
        int id;
        long telephoneNumber;
        Port port;

        public Telephone(int id, long number)
        {
            ID = id;
            TelephoneNumber = number;
        }

        public bool ConnectTo(IHaveConnection device)
        {
            Port dev = device as Port;
            if (dev == null) return false;

            if (!Connected && !dev.Connected)
            {
                port = dev;
                port.ConnectedDevice = this;
            }
            return true;
        }

        public bool Disconnect()
        {
            if (Connected)
            {
                port.ConnectedDevice = null;
                port = null;
                return true;
            }
            return false;
        }


        public bool Connected
        {
            get { return port != null; }
        }

        public IHaveConnection ConnectedDevice
        {
            get { return port; }
            set { }
        }

        public long TelephoneNumber
        {
            get { return telephoneNumber; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                else telephoneNumber = value;
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

        public void Call(long number)
        {
            if(Connected)
            {
                port.RecieveCall(number);
            }
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }


        public void RecieveCall()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<BellEventArgs> Bell;

    }
}
