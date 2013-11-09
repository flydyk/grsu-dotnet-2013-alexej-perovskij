using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Subscriber : ICanCall
    {
        int id;
        public string Name { get; set; }
        public string Address { get; set; }
        public Contract MyContract { get; set; }


        private Telephone telephone;

        public Telephone Telephone
        {
            get
            {
                return telephone;
            }
            set
            {
                if (telephone == null && value == null) return;
                if (telephone == null && value != null)
                {
                    telephone = value;
                    telephone.Bell += ListenCall;
                }
                else
                    if (value == null)
                    {
                        telephone.Bell -= ListenCall;
                        telephone = value;
                    }
                    else
                        if (value != telephone)
                        {
                            telephone = value;
                            telephone.Bell += ListenCall;
                        }
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

        public void Abort()
        {
            Telephone.Abort();
        }

        public void Call(TelephoneNumber number)
        {
            Telephone.Call(number);
        }

        public void RecieveCall()
        {

        }

        public void ListenCall(object sender, BellEventArgs e)
        {
            if (e.CallingSubscriber != null)
            {
                RecieveCall();
            }
            else Abort();
        }




    }
}
