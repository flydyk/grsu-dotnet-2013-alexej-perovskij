using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Contract
    {
        private int portID;

        public int PortID
        {
            get { return portID; }
            set { portID = value; }
        }

        private int standID;

        public int StandID
        {
            get { return standID; }
            set { standID = value; }
        }

        private TelephoneNumber telephoneNumber;

        public TelephoneNumber TelephoneNumber
        {
            get { return telephoneNumber; }
            set { telephoneNumber = value; }
        }


        private int telephoneID;

        public int TelephoneID
        {
            get { return telephoneID; }
            set { telephoneID = value; }
        }

        private Telephone telephone;

        public Telephone Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }


        private Subscriber subscriber;

        public Subscriber Subscriber
        {
            get { return subscriber; }
            set { subscriber = value; }
        }

        private Tarrif tarrif;

        public Tarrif Tarrif
        {
            get { return tarrif; }
            set { tarrif = value; }
        }



    }
}
