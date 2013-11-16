using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public interface ICallingDevice:ICanCall
    {
        event EventHandler<BellEventArgs> Bell;
    }
}
