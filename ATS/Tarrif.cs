using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public enum Tarrifs
    {
        Cheap,
        Middle,
        Expensive
    }

    public class Tarrif
    {
        readonly int MonthCost;
        readonly int MinuteCost;
        readonly bool Unlimited = false;
        Tarrifs tarrif;

        public Tarrif(Tarrifs tarrif)
        {
            TarrifType = tarrif;
            switch (TarrifType)
            {
                case Tarrifs.Cheap:
                    MonthCost = 100;
                    MinuteCost = 1;
                    break;
                case Tarrifs.Middle:
                    MonthCost = 150;
                    MinuteCost = 2;
                    break;
                case Tarrifs.Expensive:
                    MonthCost = 300;
                    MinuteCost = 0;
                    Unlimited = true;
                    break;
                default:
                    MonthCost = MinuteCost = -1;
                    break;
            }
        }

        public Tarrifs TarrifType
        {
            get { return tarrif; }
            private set { tarrif = value; }
        }
    }
}
