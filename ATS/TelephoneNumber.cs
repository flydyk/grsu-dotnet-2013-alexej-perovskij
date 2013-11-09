﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public struct TelephoneNumber:IComparable<TelephoneNumber>
    {
        public static TelephoneNumber Empty = new TelephoneNumber(0, 0);

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

        public TelephoneNumber(int portID,int standID)
        {
            this.portID = portID;
            this.standID = standID;
        }


        public override string ToString()
        {
            return string.Format("{0}-{1}", StandID, PortID);
        }

        public int CompareTo(TelephoneNumber other)
        {
            if (StandID > other.StandID) return 1;
            if (StandID == other.StandID)
            {
                return PortID.CompareTo(other.PortID);
            }
            else return -1;
        }
    }
}
