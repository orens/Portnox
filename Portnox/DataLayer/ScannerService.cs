using Portnox.Interfaces;
using Portnox.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.DataLayer
{
    public class ScannerService
    {
        private PortnoxEntities db;
        public ScannerService()
        {
            db = new PortnoxEntities();
        }

        public ScannerService(PortnoxEntities context)
        {
            db = context;
        }

        Dictionary<string, ISwitch> Switches = new Dictionary<string, ISwitch>();
        Dictionary<KeyValuePair<string, byte>, ISwitchPort> SwitchPorts = new Dictionary<KeyValuePair<string, byte>, ISwitchPort>();
        Dictionary<string, IDevice> Devices = new Dictionary<string, IDevice>();
        List<IEvent> Events = new List<IEvent>();

    
        public IEnumerable<ISwitch> GetSwitches()
        {
            Init();
            Populate();
            return Switches.Values;
        }

        private void Init()
        {
            foreach (var networkEvent in db.NetworkEvents)
            {
                GetSwitch(networkEvent.Switch_Ip, new Switch(networkEvent.Switch_Ip));
                GetSwitchPort(networkEvent, new SwitchPort(networkEvent.Switch_Ip, networkEvent.Port_Id));
                GetDevice(networkEvent.Device_MAC);
                Events.Add(new Event(networkEvent.Switch_Ip, networkEvent.Port_Id,networkEvent.Device_MAC, networkEvent.Event_Id));
            }
        }

        private void Populate()
        {
            PopulateSwitches();
            PopulateSwitchPorts();
            PopulateDevices();
        }

        private IDevice GetDevice(string device_MAC)
        {
            if (device_MAC == null)
            {
                return null;
            }
            if (!Devices.ContainsKey(device_MAC))
            {
                Devices.Add(device_MAC, new Device(device_MAC));
            }
            return Devices[device_MAC];
        }

        private ISwitchPort GetSwitchPort(NetworkEvent networkEvent, ISwitchPort switchPort = null)
        {
            if (!SwitchPorts.ContainsKey(new KeyValuePair<string, byte>(networkEvent.Switch_Ip, networkEvent.Port_Id )))
            {
                SwitchPorts.Add(new KeyValuePair<string, byte>(networkEvent.Switch_Ip, networkEvent.Port_Id), switchPort);
            }
            return SwitchPorts[new KeyValuePair<string, byte>(networkEvent.Switch_Ip, networkEvent.Port_Id)];
        }

        private ISwitch GetSwitch(string switch_Ip, ISwitch @switch = null)
        {
            if (!Switches.ContainsKey(switch_Ip))
            {
                Switches.Add(switch_Ip, @switch);
            }
            return Switches[switch_Ip];
        }

        private void PopulateSwitches()
        {
            foreach (var @switch in Switches)
            {
                @switch.Value.Ports = SwitchPorts.Where(w => w.Key.Key == @switch.Value.Switch_Ip).Select(s => s.Value);
                @switch.Value.Events = Events.Where(w => w.Switch_Ip == @switch.Value.Switch_Ip);
            }
        }

        private void PopulateSwitchPorts()
        {
            foreach (var sp in SwitchPorts)
            {
                sp.Value.Events = Events.Where(w => w.SPort == sp.Value.Port_Id);
            }
        }

        private void PopulateDevices()
        {
            foreach (var device in Devices)
            {
                device.Value.Events = Events.Where(w => w.Device_MAC == device.Value.Device_MAC);
            }
        }
    }
}
