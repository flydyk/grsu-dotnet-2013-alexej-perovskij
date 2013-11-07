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
        
        public string Owner { get; set; }

        public ATS(int id, string owner)
        {
            Owner = owner;
            ID = id;
            stands = new Dictionary<int, ATSStand>();
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

        public void SignContract(Subscriber sub, Tarrif tarrif)
        {

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

    public class IncommingCallEventArgs:EventArgs
    {
        readonly long Number;
        public IncommingCallEventArgs(long number)
        {
            Number = number;
        }
    }
}
