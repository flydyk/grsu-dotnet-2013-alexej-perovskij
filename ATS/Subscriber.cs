using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Subscriber:ICanCall
    {
        int id;
        public string Name { get; set; }
        public string Address { get; set; }
        public Telephone telephone;



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

        public void Call(int number)
        {
            throw new NotImplementedException();
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Call(long number)
        {
            throw new NotImplementedException();
        }

        public void RecieveCall()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<BellEventArgs> Bell;
    }
}
