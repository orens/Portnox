using Portnox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Interfaces
{
    public interface ISwitch
    {
        string Switch_Ip { get; set; }
        IList<ISwitchPort> Ports { get; set; }
        IList<IEvent> Events { get; set; }
    }
}
