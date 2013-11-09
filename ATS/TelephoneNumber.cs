using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public struct TelephoneNumber
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

    }
}
