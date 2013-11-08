using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public interface IConnectable
    {
        bool ConnectTo(IConnectable device);
        bool Disconnect();

        IConnectable ConnectedDevice { get; }
        bool Connected { get; }
    }
}
