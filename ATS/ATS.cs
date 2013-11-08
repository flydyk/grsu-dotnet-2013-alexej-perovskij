using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class ATS
    {
        int id;
        Dictionary<int, ATSStand> stands;
        Dictionary<TelephoneNumber, Contract> contracts;
        TelephoneNumber newTelephoneNumber;

        public string Owner { get; set; }

        public ATS(int id, string owner)
        {
            Owner = owner;
            ID = id;
            stands = new Dictionary<int, ATSStand>();
            stands.Add(0, new ATSStand(0));
            newTelephoneNumber = new TelephoneNumber();
            newTelephoneNumber.PortID = 0;
            newTelephoneNumber.StandID = 0;
        }

        public void AddStand(ATSStand stand)
        {
            if (!stands.ContainsKey(stand.ID))
                stands[stand.ID] = stand;
            else throw new ArgumentException(
                string.Format("Stand with ID: {0} already exists.", stand.ID));
        }
        public void RemoveStand(int id)
        {
            stands.Remove(id);
        }

        public Telephone SignContract(Subscriber sub, Tarrif tarrif)
        {
            TelephoneNumber number = GetTelephoneNumber();
            Contract c = new Contract()
            {
                Subscriber = sub,
                Tarrif = tarrif,
                TelephoneID = 0,
                TelephoneNumber = number,
                PortID = number.PortID,
                StandID = number.StandID
            };
            this[c.StandID][c.PortID].IncommingCall += ATS_IncommingCall;
            return new Telephone(c.TelephoneID, c.TelephoneNumber);

        }

        void ATS_IncommingCall(object sender, CallEventArgs e)
        {
            Port p = this[e.ToNumber.StandID][e.ToNumber.PortID];
            if (p.IsBusy)
            {

            }
            else
            {

            };
        }

        private TelephoneNumber GetTelephoneNumber()
        {
            newTelephoneNumber.PortID = (newTelephoneNumber.PortID + 1) % ATSStand.PORTS_COUNT;
            if (newTelephoneNumber.PortID == 0) newTelephoneNumber.StandID++;

            return newTelephoneNumber;
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
}
