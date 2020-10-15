using Portnox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Models
{
    public class Event : IEvent
    {
        public Event(string @switch, byte switchPort, string device, int event_Id)
        {
            Switch_Ip = @switch;
            SPort = switchPort;
            Device_MAC = device;
            Event_Id = event_Id;
        }
        public int Event_Id { get; set; }
        public string Switch_Ip { get; set; }
        public string Device_MAC { get; set; }
        public byte SPort { get; set; }
    }
}
