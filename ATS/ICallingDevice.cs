using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public interface ICallingDevice
    {
        void Call(TelephoneNumber number);
        void Abort();
        void AcceptCall();
        event EventHandler<BellEventArgs> Bell;
    }
}
