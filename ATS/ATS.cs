using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public enum AbortReason
    {
        ATS,
        BusyLine,
        Subsriber
    }

    public class ATS
    {
        int id;
        Dictionary<int, ATSStand> stands;
        Dictionary<TelephoneNumber, Contract> contracts;
        Dictionary<TelephoneNumber, List<Session>> statistics;
        TelephoneNumber newTelephoneNumber = TelephoneNumber.Empty;
        public string Owner { get; set; }

        public ATS(int id, string owner)
        {
            Owner = owner;
            ID = id;
            contracts = new Dictionary<TelephoneNumber, Contract>();
            statistics = new Dictionary<TelephoneNumber, List<Session>>();

            stands = new Dictionary<int, ATSStand>();
        }

        public void AddStand()
        {
            stands.Add(stands.Count, new ATSStand(stands.Count));
        }
        /*
        private void AddStand(ATSStand stand)
        {
            if (!stands.ContainsKey(stand.ID))
                stands[stand.ID] = stand;
            else throw new ArgumentException(
                string.Format("Stand with ID: {0} already exists.", stand.ID));
        }
        */

        public void RemoveStand(int id)
        {
            stands.Remove(id);
        }

        public Contract SignContract(Subscriber sub, Tarrifs tarrif)
        {
            TelephoneNumber number = GetTelephoneNumber();
            Contract contract = new Contract()
            {
                Subscriber = sub,
                Tarrif = new Tarrif(tarrif),
                Telephone = new Telephone(number),
                PortID = number.PortID,
                StandID = number.StandID,
                PayTime = DateTime.Now,
                ATS = this
            };
            sub.Contract = contract;
            sub.Telephone = contract.Telephone;

            // configure ports
            this[contract.StandID][contract.PortID].IncommingCall += ATS_IncommingCall;
            this[contract.StandID][contract.PortID].AbortCall += ATS_AbortCall;
            this[contract.StandID][contract.PortID].AcceptCallBack += ATS_AcceptCallBack;
            
            contracts.Add(number, contract);
            return contract;
        }

        void ATS_AcceptCallBack(object sender, CallBackEventArgs e)
        {
            Session s = new Session(1)
                {
                    Caller = e.Caller,
                    Taker = e.Taker,
                    StartTime = DateTime.Now,
                };

            if (!statistics.ContainsKey(s.Caller.Telephone.TelephoneNumber))
            {
                List<Session> ses = new List<Session>();
                statistics.Add(s.Caller.Telephone.TelephoneNumber, ses);
            }
            statistics[s.Caller.Telephone.TelephoneNumber].Add(s);
        }

        void ATS_AbortCall(object sender, AbortCallEventArgs e)
        {
            switch (e.Reason)
            {
                case AbortReason.ATS:
                    break;
                case AbortReason.Subsriber:
                    if (statistics.ContainsKey(e.Caller))
                    {
                        Session s = statistics[e.Caller][statistics[e.Caller].Count - 1];
                        s.EndTime = DateTime.Now;
                        int port = s.Taker.Contract.PortID;
                        int stand = s.Taker.Contract.StandID;
                        CallBackEventArgs cb = new CallBackEventArgs(false, s.Caller, s.Taker);
                        this[stand][port].GenCallBack(cb);
                        port = s.Caller.Contract.PortID;
                        stand = s.Caller.Contract.StandID;
                        this[stand][port].GenCallBack(cb);
                    }
                    else 
                    {
                        this[e.Caller.StandID][e.Caller.PortID]
                            .GenCallBack(new CallBackEventArgs(false, contracts[e.Caller].Subscriber, null));
                    }
                    break;
                default:
                    break;
            }
        }

        void ATS_IncommingCall(object sender, CallEventArgs e)
        {
            Port toPort = this[e.ToNumber.StandID][e.ToNumber.PortID];
            Port fromPort = (Port)sender;
            Subscriber caller = contracts[e.FromNumber].Subscriber;
            Subscriber taker=contracts[e.ToNumber].Subscriber;
            if (toPort.IsBusy||!toPort.Connected)
            {
                fromPort.GenCallBack(new CallBackEventArgs(false, caller, taker));
            }
            else
            {
                fromPort.GenCallBack(
                    new CallBackEventArgs(
                        true,
                        caller,
                        taker)
                    );
                toPort.GenCall(caller);
            };
        }

        TelephoneNumber GetTelephoneNumber()
        {
            newTelephoneNumber.PortID = (newTelephoneNumber.PortID + 1) % ATSStand.PORTS_COUNT;
            if (newTelephoneNumber.PortID == 0)
                newTelephoneNumber.StandID++;
            
            // add one more stand to this station if required
            if (newTelephoneNumber.StandID == stands.Count)
                stands.Add(newTelephoneNumber.StandID, new ATSStand(newTelephoneNumber.StandID));

            return newTelephoneNumber;
        }

        public List<Session> GetSessions(TelephoneNumber number)
        {
            if (statistics.ContainsKey(number))
                return new List<Session>(statistics[number]);
            else return new List<Session>();
        }

        public List<Session> SessionsFilteredByName(TelephoneNumber number, string name)
        {
            var sessions = from session in GetSessions(number)
                           where session.Taker.Name == name
                           select session;
            return sessions.ToList();
        }

        public List<Session> SessionsFilteredByCost(TelephoneNumber number, int lowBound, int highBound)
        {
            var sessions = from session in GetSessions(number)
                           where session.Cost >= lowBound && session.Cost <= highBound
                           select session;
            return sessions.ToList();
        }

        public List<Session> SessionsFilteredByDate(TelephoneNumber number, DateTime lowBound, DateTime highBound)
        {
            var sessions = from session in GetSessions(number)
                           where session.EndTime >= lowBound && session.EndTime <= highBound
                           select session;
            return sessions.ToList();
        }
        /// <summary>
        /// Get ATSStand by ID 
        /// </summary>
        /// <param name="id">ID of the ATSStaion</param>
        /// <returns>ATSStand of the ATS</returns>
        public ATSStand this[int id]
        {
            get { return stands[id]; }
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
    }




    public class CallEventArgs:EventArgs
    {
        public readonly TelephoneNumber FromNumber;
        public readonly TelephoneNumber ToNumber;

        public CallEventArgs(TelephoneNumber thisNumber, TelephoneNumber thatNumber)
        {
            FromNumber = thisNumber;
            ToNumber = thatNumber;
        }
    }

    public class CallBackEventArgs : EventArgs
    {
        public readonly bool Accepted;
        public readonly Subscriber Caller;
        public readonly Subscriber Taker;
        public CallBackEventArgs(bool accepted,Subscriber caller,Subscriber taker)
        {
            Caller = caller;
            Taker = taker;
            Accepted = accepted;
        }
    }

    public class AbortCallEventArgs : EventArgs
    {
        public readonly AbortReason Reason;
        public readonly TelephoneNumber Caller;

        public AbortCallEventArgs(AbortReason reason,TelephoneNumber caller)
        {
            Caller = caller;
            Reason = reason;
        }
    }

    public class BellEventArgs : EventArgs
    {
        public readonly Subscriber CallingSubscriber;

        public BellEventArgs(Subscriber s)
        {
            CallingSubscriber = s;
        }
    }
    /*
    public class RecieveCallBackEventArgs : EventArgs
    {
        public readonly bool Accepted;
        public readonly Session Session;
        public RecieveCallBackEventArgs(bool accepted,Session session)
        {
            Session = session;
            Accepted = accepted;
        }
    }*/
}
