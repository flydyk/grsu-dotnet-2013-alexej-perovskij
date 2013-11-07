using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATS
{
    public class BellEventArgs:EventArgs
    {
        readonly Subscriber subscriber;
        public BellEventArgs(Subscriber s)
        {
            subscriber = s;
        }
    }
}
