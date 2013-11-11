using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public enum LineSingnal
    {
        None,
        ATSAbort,
        BusyLine,
        SubsriberAbort,
        Established,
        Accept
    }

    public class ATS
    {
        int id;
        Dictionary<int, ATSStand> stands;
        Dictionary<TelephoneNumber, Contract> contracts;
        Dictionary<TelephoneNumber, List<Session>> statistics;
        Dictionary<int, Session> currentSessions;
        int sessionID;

        TelephoneNumber newTelephoneNumber = TelephoneNumber.Empty;
        public string Owner { get; set; }

        public ATS(int id, string owner)
        {
            Owner = owner;
            ID = id;
            contracts = new Dictionary<TelephoneNumber, Contract>();
            statistics = new Dictionary<TelephoneNumber, List<Session>>();
            currentSessions = new Dictionary<int, Session>();

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
            TelephoneNumber number = NewTelephoneNumber();
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

            statistics.Add(number, new List<Session>());
            contracts.Add(number, contract);
            return contract;
        }

        void ATS_AcceptCallBack(object sender, CallEventArgs e)
        {
            currentSessions[e.SessionID].StartTime = DateTime.Now;
        }

        void ATS_AbortCall(object sender, AbortCallEventArgs e)
        {
            switch (e.Reason)
            {
                case LineSingnal.ATSAbort:
                    break;
                case LineSingnal.SubsriberAbort:
                    Session s = currentSessions[e.SessionID];
                    s.EndTime = DateTime.Now;

                    TelephoneNumber caller = s.Caller.Telephone.TelephoneNumber;
                    TelephoneNumber taker = s.Taker.Telephone.TelephoneNumber;

                    TelephoneNumber target = (e.Aborter == caller) ? taker : caller;

                    statistics[caller].Add(s);
                    currentSessions.Remove(e.SessionID);
                    CallEventArgs cb = new CallEventArgs(-1, e.Reason, caller, e.Aborter);

                    this[target.StandID][target.PortID].GenCallBack(cb);

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
                fromPort.GenCallBack(new CallEventArgs(-1, LineSingnal.BusyLine, e.Caller, e.Taker));
            }
            else
            {
                CallEventArgs cea = new CallEventArgs(NewSession(e), LineSingnal.Established, e.Caller, e.Taker);
                fromPort.GenCallBack(cea);
                toPort.GenCall(cea);
            };
        }

        int NewSessionID()
        {
            return sessionID++;
        }

        int NewSession(CallEventArgs e)
        {
            Session s = new Session(NewSessionID())
            {
                Caller = contracts[e.Caller].Subscriber,
                Taker = contracts[e.Taker].Subscriber,
            };
            currentSessions.Add(s.ID, s);

            return s.ID;
        }

        TelephoneNumber NewTelephoneNumber()
        {
            newTelephoneNumber.PortID = (newTelephoneNumber.PortID + 1) % ATSStand.PORTS_COUNT;
            if (newTelephoneNumber.PortID == 0)
                newTelephoneNumber.StandID++;
            
            // add one more stand to this station if required
            if (newTelephoneNumber.StandID == stands.Count)
                stands.Add(newTelephoneNumber.StandID, new ATSStand(newTelephoneNumber.StandID));

            return newTelephoneNumber;
        }

        #region Selectors of sessions
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
        #endregion

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
        public readonly LineSingnal Signal;
        public readonly TelephoneNumber Caller;
        public readonly TelephoneNumber Taker;
        public readonly int SessionID;

        public CallEventArgs(int sessionID,LineSingnal signal, TelephoneNumber caller, TelephoneNumber taker)
        {
            Caller = caller;
            Taker = taker;
            Signal = signal;
            SessionID = sessionID;
        }

        public CallEventArgs(TelephoneNumber caller, TelephoneNumber taker)
            : this(-1, LineSingnal.None, caller, taker)
        {
        }
        public CallEventArgs(int sessionID, LineSingnal signal)
            : this(sessionID, LineSingnal.None, TelephoneNumber.Empty, TelephoneNumber.Empty)
        {
        }

    }

    public class AbortCallEventArgs : EventArgs
    {
        public readonly LineSingnal Reason;
        public readonly int SessionID;
        public readonly TelephoneNumber Aborter;

        public AbortCallEventArgs(int sessionID, LineSingnal reason, TelephoneNumber aborter)
        {
            SessionID = sessionID;
            Reason = reason;
            Aborter = aborter;
        }
    }

    public class BellEventArgs : EventArgs
    {
        public readonly TelephoneNumber Caller;
        public readonly int SessionID;

        public BellEventArgs(int sessionID, TelephoneNumber caller)
        {
            SessionID = sessionID;
            Caller = caller;
        }
    }
}
