using Portnox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Models
{
    public class SwitchPort : ISwitchPort
    {
        public SwitchPort(string switch_Ip, byte port_Id)
        {
            Switch_Ip = switch_Ip;
            Port_Id = port_Id;
            Events = new List<IEvent>();
            Devices = new Dictionary<string, IDevice>();
        }
        public IDictionary<string,IDevice> Devices { get; set; }
        public IList<IEvent> Events { get; set; }
        public byte Port_Id { get; set; }
        public string Switch_Ip { get; set; }
    }
}
