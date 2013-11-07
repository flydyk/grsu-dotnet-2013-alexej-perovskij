using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    class Program
    {
        static void Main(string[] args)
        {
            ATS ats = new ATS(10, "Owner");
            ats.AddStand(new ATSStand(1));
            
        }
    }
}
