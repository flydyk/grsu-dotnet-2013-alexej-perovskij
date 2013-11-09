using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public interface ICanCall
    {
        void Call(TelephoneNumber number);
        void Abort();
        void RecieveCall(Subscriber caller);
    }

    public interface ICallingDevice : ICanCall
    {
        void RecieveCall(Subscriber taker, Subscriber caller);
        //void RecieveCall(object sender,BellEventArgs e);
        event EventHandler<BellEventArgs> Bell;
    }
}
