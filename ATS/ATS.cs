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

        void ATS_AcceptCallBack(object sender, CallEventArgs e)
        {
            Session s = new Session(1)
                {
                    Caller = contracts[e.Caller].Subscriber,
                    Taker = contracts[e.Taker].Subscriber,
                    StartTime = DateTime.Now,
                };

            if (!statistics.ContainsKey(e.Caller))
            {
                List<Session> ses = new List<Session>();
                statistics.Add(e.Caller, ses);
            }
            statistics[e.Caller].Add(s);
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
                        CallEventArgs cb = new CallEventArgs(false, e.Caller, s.Taker.Telephone.TelephoneNumber);
                        this[stand][port].GenCallBack(cb);
                        port = e.Caller.PortID;
                        stand = e.Caller.StandID;
                        this[stand][port].GenCallBack(cb);
                    }
                    else 
                    {
                        this[e.Caller.StandID][e.Caller.PortID]
                            .GenCallBack(new CallEventArgs(false, e.Caller, TelephoneNumber.Empty));
                    }
                    break;
                default:
                    break;
            }
        }

        void ATS_IncommingCall(object sender, CallEventArgs e)
        {
            Port toPort = this[e.Taker.StandID][e.Taker.PortID];
            Port fromPort = (Port)sender;
            if (toPort.IsBusy || !toPort.Connected)
            {
                fromPort.GenCallBack(new CallEventArgs(false, e.Caller, e.Taker));
            }
            else
            {
                fromPort.GenCallBack(new CallEventArgs(true, e.Caller, e.Taker));
                toPort.GenCall(e.Caller);
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




    public class CallEventArgs : EventArgs
    {
        public readonly bool Accepted;
        public readonly TelephoneNumber Caller;
        public readonly TelephoneNumber Taker;

        public CallEventArgs(bool accepted, TelephoneNumber caller, TelephoneNumber taker)
        {
            Caller = caller;
            Taker = taker;
            Accepted = accepted;
        }

        public CallEventArgs(TelephoneNumber caller, TelephoneNumber taker)
            : this(true, caller, taker)
        {
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
        public readonly TelephoneNumber Caller;

        public BellEventArgs(TelephoneNumber caller)
        {
            Caller = caller;
        }
    }
}
