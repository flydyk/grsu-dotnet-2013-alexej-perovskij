using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Port : IConnectable
    {
        int id;
        public bool IsBusy { get; set; }
        IConnectable dev = null;
        public event EventHandler<IncommingCallEventArgs> IncommingCall;

        public Port(int id)
        {
            ID = id;
        }

        public void RecieveCall(TelephoneNumber thisNumber, TelephoneNumber thatNumber)
        {
            IncommingCall(this, new IncommingCallEventArgs(thisNumber, thatNumber));
        }
        public void OutCommingCall() { }

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

        #region IConnectable
        public bool ConnectTo(IConnectable device)
        {
            if (Connected) return false;

            dev = device;
            if (dev.ConnectedDevice == this) return true;

            if (!device.ConnectTo(this))
            {
                dev = null;
                return false;
            }
            return true;
        }

        public bool Disconnect()
        {
            if (Connected)
            {
                IConnectable temp = dev;
                dev = null;
                temp.Disconnect();
                temp = null;
                return true;
            }
            return false;
        }

        public bool Connected
        {
            get { return dev != null; }
        }

        public IConnectable ConnectedDevice
        {
            get { return dev; }
        }
        #endregion

    }

}
