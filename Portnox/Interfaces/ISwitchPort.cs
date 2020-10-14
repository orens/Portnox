using Portnox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Interfaces
{
    public interface ISwitchPort
    {
        Byte Port_Id { get; set; }
        IEnumerable<IDevice> Devices { get; set; }
        IEnumerable<IEvent> Events { get; set; }
    }
}
