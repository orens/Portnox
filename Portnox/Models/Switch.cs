using Portnox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Models
{
    public class Switch : ISwitch
    {
        public Switch(string switch_Ip)
        {
            Switch_Ip = switch_Ip;
            Events = new List<IEvent>();
            Ports = new List<ISwitchPort>();
        }
        public string Switch_Ip { get; set; }
        public IList<ISwitchPort> Ports { get; set; }
        public IList<IEvent> Events { get; set; }
    }
}
