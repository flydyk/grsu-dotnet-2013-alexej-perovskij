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
                //port.GenerateCall += RecieveCall;
                //port.CallBack += port_CallBack; 
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
                port.Abort(AbortReason.Subsriber, lastCaller == TelephoneNumber ? lastCaller : TelephoneNumber.Empty);
                
            }
        }

        public void AcceptCall(TelephoneNumber taker, TelephoneNumber caller)
        {
            // 3
            port.GenAcceptCallBack(taker, caller);
        }
        #endregion

        private void RecieveCall(object sender, BellEventArgs e)
        {
            currentCaller = e.Caller;
            Bell(this, e);
        }



        void port_CallBack(object sender, CallEventArgs e)
        {
            if (e.Accepted)
            {
                Console.WriteLine(" {0}, connection was established with # {1} #. Wait for taker..", e.Caller, e.Taker);
            }
            else
            {
                if (e.Taker == TelephoneNumber.Empty)
                    Console.WriteLine("Taker abort call of {0}", e.Caller);
                else
                    Console.WriteLine("{0}, line to {1} is busy or other reason.",
                        e.Caller == TelephoneNumber ? e.Caller : e.Taker,
                        e.Caller == TelephoneNumber ? e.Taker : e.Caller
                        );
            }
        }


        

        
    }
}
