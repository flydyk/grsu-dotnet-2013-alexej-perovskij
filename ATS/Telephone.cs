﻿using System;
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
        TelephoneNumber currentCaller = TelephoneNumber.Empty;
        Port port;

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


        public void Call(TelephoneNumber number)
        {
            if(Connected)
            {
                currentCaller = telephoneNumber;
                port.RecieveCall(TelephoneNumber,number);
            }
        }

        public void Abort()
        {
            if(Connected)
            {
                port.Abort(AbortReason.Subsriber, currentCaller);
                currentCaller = TelephoneNumber.Empty;
            }
        }


        private void RecieveCall(object sender, BellEventArgs e)
        {
            currentCaller = e.CallingSubscriber.Telephone.TelephoneNumber;
            Bell(this, e);
        }

        public event EventHandler<BellEventArgs> Bell;

        void port_CallBack(object sender, CallBackEventArgs e)
        {
            Console.WriteLine("Line is free or end of conversaition on {1}: {0}",
                !e.Accepted, TelephoneNumber.ToString());

        }


        public void RecieveCall(Subscriber taker,Subscriber caller)
        {
            port.GenAcceptCallBack(taker,caller);
        }

        
        public void RecieveCall(Subscriber caller)
        {
            throw new NotImplementedException();
        }
    }
}
