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
        TelephoneNumber lastCaller = TelephoneNumber.Empty;
        TelephoneNumber currentCaller = TelephoneNumber.Empty;
        Port port;
        int sessionID;

        public event EventHandler<CallEventArgs> Calling;


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
                currentCaller = TelephoneNumber;
                // 1
                port.RecieveCall(TelephoneNumber, number);
                //Calling(this, new CallEventArgs(TelephoneNumber, number));
            }
        }

        public void Abort()
        {
            if(Connected)
            {
                // 2
                lastCaller = currentCaller;
                port.Abort(sessionID, LineSingnal.SubsriberAbort, TelephoneNumber);
                sessionID = -1;
                
            }
        }

        public void AcceptCall()
        {
            // 3
            port.GenAcceptCallBack(sessionID);
        }
        #endregion

        private void RecieveCall(object sender, BellEventArgs e)
        {
            sessionID = e.SessionID;
            currentCaller = e.Caller;
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
                    Console.WriteLine("{0}, Line is busy to {1} or other reason", e.Caller, e.Taker);
                    break;
                case LineSingnal.SubsriberAbort:
                    Console.WriteLine("{0} abort call of {0}", e.Taker, e.Caller);
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
