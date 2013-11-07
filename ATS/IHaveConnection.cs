using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public interface IHaveConnection
    {
        IHaveConnection ConnectedDevice { get; set; }
        bool Connected { get; }
    }
}
