using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Session
    {
        int id;
        DateTime endTime;

        public Subscriber Caller { get; set; }
        public Subscriber Taker { get; set; }
        public int Cost { get; private set; }
        public TimeSpan TalkTime { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime
        {
            get { return endTime; }
            set 
            {
                endTime = value;
                TalkTime = endTime - StartTime;
                Cost = Caller.Contract.Tarrif.MinuteCost * TalkTime.Seconds;
            }
        }
        

        public Session(int id)
        {
            ID = id;
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

        public override string ToString()
        {
            return string.Format("[Subsriber: {0}, TalkTime: {1}, Cost: {2}",
                Taker.Name, TalkTime.Seconds, Cost);
        }
    }
}
