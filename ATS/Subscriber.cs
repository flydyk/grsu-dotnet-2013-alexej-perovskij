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
        public Contract Contract { get; set; }

        private Telephone telephone;

        public Subscriber(int id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
        }

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

        #region ICanCall
        public void Abort()
        {
            Telephone.Abort();
        }

        public void Call(TelephoneNumber number)
        {
            Telephone.Call(number);
        }

        public void RecieveCall(Subscriber caller)
        {
            telephone.AcceptCall(this, caller);
        }

        private void ListenCall(object sender, BellEventArgs e)
        {
            Console.Write("{0} is calling to {1}\nRecieve call? (y/n)",
                e.CallingSubscriber.Name, Name);
            string todo = Console.ReadLine();
            if (todo == "y")
                RecieveCall(e.CallingSubscriber);
            else
                Abort();
        }
        #endregion


        public List<Session> GetSessions()
        {
            return Contract.ATS.GetSessions(Telephone.TelephoneNumber);
        }
        public List<Session> SessionsFilteredByName(string name)
        {
            return Contract.ATS.SessionsFilteredByName(Telephone.TelephoneNumber, name);
        }
        public List<Session> SessionsFilteredByCost(int lowBound, int highBound)
        {
            return Contract.ATS.SessionsFilteredByCost(Telephone.TelephoneNumber, lowBound, highBound);
        }
        public List<Session> SessionsFilteredByDate(DateTime lowBound, DateTime highBound)
        {
            return Contract.ATS.SessionsFilteredByDate(Telephone.TelephoneNumber, lowBound, highBound);
        }

    }
}
