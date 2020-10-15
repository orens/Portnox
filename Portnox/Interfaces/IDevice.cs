using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Interfaces
{
    public interface IDevice
    {
        string Device_MAC { get; set; }
        IList<IEvent> Events { get; set; }
    }
}
