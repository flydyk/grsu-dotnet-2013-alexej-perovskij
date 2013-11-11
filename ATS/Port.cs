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
        public event EventHandler<CallEventArgs> IncommingCall;
        public event EventHandler<CallEventArgs> CallBack;
        public event EventHandler<AbortCallEventArgs> AbortCall;
        public event EventHandler<BellEventArgs> GenerateCall;
        public event EventHandler<CallEventArgs> AcceptCallBack;

        public Port(int id)
        {
            ID = id;
        }

        public void RecieveCall(TelephoneNumber caller, TelephoneNumber taker)
        {
            if (IncommingCall != null)
            {
                IsBusy = true;
                IncommingCall(this, new CallEventArgs(caller, taker));
            }
        }
        public void GenCall(CallEventArgs sub)
        {
            Telephone t = dev as Telephone;
            if (t != null && GenerateCall != null)
            {
                IsBusy = true;
                GenerateCall(this, new BellEventArgs(sub.SessionID, sub.Caller));
            }
        }

        public void Abort(int sessionID, LineSingnal reason, TelephoneNumber aborter)
        {
            if (AbortCall != null)
            {
                IsBusy = false;
                if (sessionID != -1)
                    AbortCall(this, new AbortCallEventArgs(sessionID, reason, aborter));
            }
        }

        public void GenCallBack(CallEventArgs e)
        {
            if (CallBack != null)
            {
                
                CallBack(this, e);
            }
        }

        public void GenAcceptCallBack(int sessionID)
        {
            if (AcceptCallBack != null)
            {
                CallEventArgs e = new CallEventArgs(sessionID, LineSingnal.Accept);
                AcceptCallBack(this, e);
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

        #region IConnectable
        public bool ConnectTo(IConnectable device)
        {
            if (Connected) return false;



            dev = device;
            if (dev.ConnectedDevice == this)
            { 

                return true; }

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
