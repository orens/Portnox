using Portnox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Models
{
    public class Device : IDevice
    {
        public Device(string device_mac)
        {
            Device_MAC = device_mac;    
        }
        public string Device_MAC { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}
