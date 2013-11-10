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
        Port port;

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
            if (port.ConnectedDevice == this) return true;

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
                lastCaller = TelephoneNumber;
                port.RecieveCall(TelephoneNumber,number);
            }
        }

        public void Abort()
        {
            if(Connected)
            {
                port.Abort(AbortReason.Subsriber, lastCaller);
                //lastCaller = TelephoneNumber.Empty;
            }
        }

        public void AcceptCall(Subscriber taker, Subscriber caller)
        {
            port.GenAcceptCallBack(taker, caller);
        }
        #endregion

        private void RecieveCall(object sender, BellEventArgs e)
        {
            lastCaller = e.CallingSubscriber.Telephone.TelephoneNumber;
            Bell(this, e);
        }

        

        void port_CallBack(object sender, CallBackEventArgs e)
        {
            if (e.Accepted)
            {
                Console.WriteLine(" {0}, connection was established with # {1} #. Wait for taker..", e.Caller.Name, e.Taker.Name);
            }
            else
            {
                if (e.Taker == null)
                    Console.WriteLine("Taker abort call of {0}", e.Caller.Name);
                else
                    Console.WriteLine("{0}, line to {1} is busy or other reason.",
                        e.Caller.Telephone.TelephoneNumber.CompareTo(TelephoneNumber) == 0 ? e.Caller.Name : e.Taker.Name,
                        e.Caller.Telephone.TelephoneNumber.CompareTo(TelephoneNumber) == 0 ? e.Taker.Name : e.Caller.Name
                        );
            }
        }


        

        
    }
}
