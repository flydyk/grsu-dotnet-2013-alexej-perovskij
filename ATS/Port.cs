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
        Telephone dev = null;
        public event EventHandler<CallEventArgs> IncommingCall;
        public event EventHandler<CallEventArgs> CallBack;
        public event EventHandler<AbortCallEventArgs> AbortCall;
        public event EventHandler<BellEventArgs> GenerateCall;
        public event EventHandler<CallEventArgs> AcceptCallBack;

        public Port(int id)
        {
            ID = id;
        }

        public void GenCall(CallEventArgs sub)
        {
            if (GenerateCall != null)
            {
                IsBusy = true;
                GenerateCall(this, new BellEventArgs(sub.SessionID, sub.Caller));
            }
        }

        #region Hiden Methods for telephone
        /*
        public void RecieveCall(TelephoneNumber caller, TelephoneNumber taker)
        {
            if (IncommingCall != null)
            {
                IsBusy = true;
                IncommingCall(this, new CallEventArgs(caller, taker));
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

        public void GenAcceptCallBack(int sessionID)
        {
            if (AcceptCallBack != null)
            {
                CallEventArgs e = new CallEventArgs(sessionID, LineSingnal.Accept);
                AcceptCallBack(this, e);
            }
        }
        */
        #endregion

        public void GenCallBack(CallEventArgs e)
        {
            if (CallBack != null)
            {                
                CallBack(this, e);
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

            Telephone _dev = device as Telephone;
            if (_dev == null) return false;

            dev = _dev;
            if (dev.ConnectedDevice == this)
            {
                dev.Calling += dev_Calling;
                dev.Acepting += dev_Acepting;
                dev.Aborting += dev_Aborting;
                return true; 
            }

            if (!device.ConnectTo(this))
            {
                dev = null;
                return false;
            }
            dev.Calling += dev_Calling;
            dev.Acepting += dev_Acepting;
            dev.Aborting += dev_Aborting;

            return true;
        }

        public bool Disconnect()
        {
            if (Connected)
            {
                Telephone temp = dev;
                dev = null;
                temp.Disconnect();

                temp.Calling -= dev_Calling;
                temp.Acepting -= dev_Acepting;
                temp.Aborting -= dev_Aborting;
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

        void dev_Aborting(object sender, AbortCallEventArgs e)
        {
            if (AbortCall != null)
            {
                IsBusy = false;
                if (e.SessionID != -1)
                    AbortCall(this, e);
            }
        }

        void dev_Acepting(object sender, CallEventArgs e)
        {
            if (AcceptCallBack != null)
            {                
                AcceptCallBack(this, e);
            }
        }

        void dev_Calling(object sender, CallEventArgs e)
        {
            if (IncommingCall != null)
            {
                IsBusy = true;
               IncommingCall(this, e);
            }
        }


    }

}
