using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    
    public class Telephone : IConnectable,ICallingDevice
    {
        int id;
        TelephoneNumber telephoneNumber;
        TelephoneNumber lastIncomingCall = TelephoneNumber.Empty;
        TelephoneNumber currentIncomingCall = TelephoneNumber.Empty;
        TelephoneNumber missedCall = TelephoneNumber.Empty;
        Port port;
        int sessionID = -1;

        public event EventHandler<CallEventArgs> Calling;
        public event EventHandler<AbortCallEventArgs> Aborting;
        public event EventHandler<CallEventArgs> Acepting;

        public event EventHandler<BellEventArgs> Bell;

        public Telephone(TelephoneNumber number)
        {
            ID = number.PortID + number.StandID * 100;
            TelephoneNumber = number;
        }

        public TelephoneNumber TelephoneNumber
        {
            get { return telephoneNumber; }
            set
            { telephoneNumber = value; }
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


        #region IConnactable
        public bool ConnectTo(IConnectable device)
        {
            if (Connected) return false;

            Port dev = device as Port;
            if (dev == null) return false;

            port = dev;
            if (port.ConnectedDevice == this)
            {
                port.GenerateCall += RecieveCall;
                port.CallBack += port_CallBack;
                return true;
            }

            if (!dev.ConnectTo(this))
            {
                port = null;
                return false;
            }
            port.GenerateCall += RecieveCall;
            port.CallBack += port_CallBack;

            return true;
        }

        

        public bool Disconnect()
        {
            if (Connected)
            {
                Port temp = port;
                port = null;
                temp.Disconnect();

                temp.GenerateCall -= RecieveCall;
                temp.CallBack -= port_CallBack;
                temp = null;
                return true;
            }
            return false;
        }


        public bool Connected
        {
            get { return port != null; }
        }

        public IConnectable ConnectedDevice
        {
            get { return port; }
        }
        #endregion

        #region ICallingDevice
        public void Call(TelephoneNumber number)
        {
            if(Connected)
            {
               // AsyncCall
               // CallRequest(number);
                
                Calling(this, new CallEventArgs(TelephoneNumber, number));
            }
        }

        #region AsyncCall
        protected virtual void CallRequest(TelephoneNumber number)
        {
            IAsyncResult ar = Calling.BeginInvoke(this, new CallEventArgs(TelephoneNumber, number), PortCallBack, null);
            
        }

        private void PortCallBack(IAsyncResult ar)
        {
            if (Calling != null)
            {
                Calling.EndInvoke(ar);
            }
        }
        #endregion

        public void Abort()
        {
            if(Connected)
            {
                lastIncomingCall = currentIncomingCall;
                Aborting(this, new AbortCallEventArgs(sessionID, LineSingnal.SubsriberAbort, TelephoneNumber));
                sessionID = -1;
                currentIncomingCall = TelephoneNumber.Empty;
                
            }
        }

        public void AcceptCall()
        {
            if (Connected)
            {
                lastIncomingCall = currentIncomingCall;
                Acepting(this, new CallEventArgs(sessionID, LineSingnal.Accept));
            }
        }
        #endregion

        void RecieveCall(object sender, BellEventArgs e)
        {
            sessionID = e.SessionID;
            currentIncomingCall = e.Caller;
            Bell(this, e);
        }


        void port_CallBack(object sender, CallEventArgs e)
        {
            sessionID = e.SessionID;
            switch (e.Signal)
            {
                case LineSingnal.None:
                    break;
                case LineSingnal.ATSAbort:
                    break;
                case LineSingnal.BusyLine:
                    Console.WriteLine("{0}, Line is busy to {1} or other reason", TelephoneNumber, e.Taker);
                    break;
                case LineSingnal.SubsriberAbort:
                    Console.WriteLine("Abort call");
                    break;
                case LineSingnal.Established:
                    Console.WriteLine(" {0}, connection was established with # {1} #. Wait for taker..", e.Caller, e.Taker);
                    break;
                case LineSingnal.Accept:
                    break;
                default:
                    break;
            }
        }


        

        
    }
}
