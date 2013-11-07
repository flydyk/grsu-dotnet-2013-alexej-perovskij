using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Port : IHaveConnection
    {
        int id;
        public bool IsBusy { get; set; }
        IHaveConnection dev = null;
        public event EventHandler<IncommingCallEventArgs> IncommingCall;

        public Port(int id)
        {
            ID = id;
        }

        public void RecieveCall(long number)
        {
            IncommingCall(this, new IncommingCallEventArgs(number));
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
        

        public bool Connected
        {
            get { return dev != null; }
        }

        public IHaveConnection ConnectedDevice
        {
            get
            {
                return dev;
            }
            set
            {
                dev = value;
            }
        }
    }

}
