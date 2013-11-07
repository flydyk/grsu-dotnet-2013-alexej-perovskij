using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Port
    {
        int id;

        public Port(int id)
        {
            ID = id;
        }

        public int ID
        {
            get { return id; }
            private set
            {
                if (value > 0)
                    id = value;
                else throw new ArgumentOutOfRangeException("ID value must be greater than zero");
            }
        }

    }
}
