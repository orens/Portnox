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

        public IEnumerable<ISwitch> GetAllSwitches()
        {
            foreach (var networkEvent in db.NetworkEvents)
            {
                ProcessEvent(networkEvent);
            }
            return Switches.Values;
        }

        private void ProcessEvent(NetworkEvent ne)
        {
            if (!Switches.ContainsKey(ne.Switch_Ip))
            {
                Switches.Add(ne.Switch_Ip, new Switch(ne.Switch_Ip));
            }

            if (!Switches[ne.Switch_Ip].Ports.Any(a => a.Port_Id == ne.Port_Id))
            {
                Switches[ne.Switch_Ip].Ports.Add(new SwitchPort(ne.Switch_Ip, ne.Port_Id));
            }

            Switches[ne.Switch_Ip].Events.Add(new Event(ne.Switch_Ip, ne.Port_Id, ne.Device_MAC, ne.Event_Id));

            PopulateDevices(ne);
            PopulateEvents(ne);
        }

        private void PopulateDevices(NetworkEvent ne)
        {
            if (ne.Device_MAC != null)
            {
                if (!Switches[ne.Switch_Ip].Ports.First(w => w.Port_Id == ne.Port_Id).Devices.Any(a => a.Device_MAC == ne.Device_MAC))
                {
                    Switches[ne.Switch_Ip].Ports.First(w => w.Port_Id == ne.Port_Id).Devices.Add(new Device(ne.Device_MAC));
                }
                Switches[ne.Switch_Ip].Ports.Where(w => w.Port_Id == ne.Port_Id).First().Devices.First(f => f.Device_MAC == ne.Device_MAC).Events.Add(new Event(ne.Switch_Ip, ne.Port_Id, ne.Device_MAC, ne.Event_Id));
            }
        }

        private void PopulateEvents(NetworkEvent ne)
        {
            Switches[ne.Switch_Ip].Ports.First(w => w.Port_Id == ne.Port_Id).Events.Add(new Event(ne.Switch_Ip, ne.Port_Id, ne.Device_MAC, ne.Event_Id));
        }
    }
}
