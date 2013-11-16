using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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
        private Timer t;
        const int secondsInMonth = 3;

        public ATS(int id, string owner)
        {
            Owner = owner;
            ID = id;
            contracts = new Dictionary<TelephoneNumber, Contract>();
            statistics = new Dictionary<TelephoneNumber, List<Session>>();
            currentSessions = new Dictionary<int, Session>();
            stands = new Dictionary<int, ATSStand>();
            t = new Timer(secondsInMonth * 1000);
            t.AutoReset = true;
            t.Enabled = true;
            t.Elapsed += t_Elapsed;
            t.Start(); 
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (contracts.Count > 0)
            {
                foreach (var c in contracts)
                {
                    c.Value.ToPay += c.Value.Tarrif.MonthCost + GetTotalCost(c.Key);
                }
            }
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
                TarrifChanged = DateTime.Now,
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
            if (toPort.IsBusy || !toPort.Connected || contracts[e.Caller].ToPay > 0)
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
        public IEnumerable<Session> GetSessions(TelephoneNumber number)
        {
            if (statistics.ContainsKey(number))
                return statistics[number];
            else return new List<Session>();
        }

        public IEnumerable<Session> SessionsFilteredByName(TelephoneNumber number, string name)
        {
            return GetSessions(number).FilterByName(name);
        }

        public IEnumerable<Session> SessionsFilteredByCost(TelephoneNumber number, int lowBound, int highBound)
        {
            return GetSessions(number).FilterByCost(lowBound, highBound);
        }

        public IEnumerable<Session> SessionsFilteredByDate(TelephoneNumber number, DateTime lowBound, DateTime highBound)
        {
            return GetSessions(number).FilterByDate(lowBound, highBound);
        }
        #endregion

        public int GetTotalCost(TelephoneNumber number)
        {
            return GetSessions(number).Sum(x => x.Cost);
        }

        public void ChangeTarrif(TelephoneNumber number, Tarrifs newTarrif)
        {
            if (!contracts.ContainsKey(number))
                throw new KeyNotFoundException(string.Format("There is no subscriber with number {0}", number));

            Contract c = contracts[number];
            if ((c.TarrifChanged - DateTime.Now).Seconds > secondsInMonth)
            {
                c.Tarrif = new Tarrif(newTarrif);
                c.TarrifChanged = DateTime.Now;
            }
        }

        public void Pay(TelephoneNumber number, int sum)
        {
            if (!contracts.ContainsKey(number))
                throw new KeyNotFoundException(string.Format("There is no subscriber with number {0}", number));
            
            Contract c = contracts[number];
            c.ToPay -= sum;
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


    public static class SessionFilters
    {
        public static IEnumerable<Session> FilterByName(this IEnumerable<Session> s, string name)
        {
            var sessions = from session in s
                           where session.Taker.Name == name
                           select session;
            return sessions;
        }

        public static IEnumerable<Session> FilterByCost(this IEnumerable<Session> s, int lowBound, int highBound)
        {
            var sessions = from session in s
                           where session.Cost >= lowBound && session.Cost <= highBound
                           select session;
            return sessions;
        }

        public static IEnumerable<Session> FilterByDate(this IEnumerable<Session> s, DateTime lowBound, DateTime highBound)
        {
            var sessions = from session in s
                           where session.EndTime >= lowBound && session.EndTime <= highBound
                           select session;
            return sessions;
        }
    }
}
