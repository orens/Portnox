using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Interfaces
{
    public interface IEvent
    {
        int Event_Id { get; set; }
        string Switch_Ip { get; set; }
        byte SPort { get; set; }
        string Device_MAC { get; set; }
    }
}
