using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Contract
    {
        public int PortID { get; set; }
        public int StandID { get; set; }
        public Telephone Telephone { get; set; }
        public Subscriber Subscriber { get; set; }
        public Tarrif Tarrif { get; set; }
        public DateTime TarrifChanged { get; set; }
        public ATS ATS { get; set; }
    }
}
