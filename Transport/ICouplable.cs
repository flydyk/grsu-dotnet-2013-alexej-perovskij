using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public interface ICouplable
    {
        void Couple(ICouplable that, Coupler c);
        void Decouple(Coupler c);
        
        ICouplable FrontCoupler { get; set; }
        ICouplable BackCoupler { get; set; }
    }

    public enum Coupler
    {
        Front,
        Back
    }
}
