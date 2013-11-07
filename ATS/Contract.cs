using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Contract
    {
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
