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

    
        public IEnumerable<ISwitch> GetSwitches()
        {
            Init();
            return Switches.Values;
        }

        private void Init()
        {
            foreach (var networkEvent in db.NetworkEvents)
            {
                GetSwitch(networkEvent);
            }
        }
        private ISwitch GetSwitch(NetworkEvent ne)
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
            return Switches[ne.Switch_Ip];
        }

        private void PopulateDevices(NetworkEvent ne)
        {
            if (ne.Device_MAC != null)
            {
                if (!Switches[ne.Switch_Ip].Ports.Where(w => w.Port_Id == ne.Port_Id).First().Devices.ContainsKey(ne.Device_MAC))
                {
                    Switches[ne.Switch_Ip].Ports.Where(w => w.Port_Id == ne.Port_Id).First().Devices.Add(ne.Device_MAC, new Device(ne.Device_MAC));
                }
                Switches[ne.Switch_Ip].Ports.Where(w => w.Port_Id == ne.Port_Id).First().Devices[ne.Device_MAC].Events.Add(new Event(ne.Switch_Ip, ne.Port_Id, ne.Device_MAC, ne.Event_Id));
            }
        }

        private void PopulateEvents(NetworkEvent ne)
        {
            Switches[ne.Switch_Ip].Ports.Where(w => w.Port_Id == ne.Port_Id).First().Events.Add(new Event(ne.Switch_Ip, ne.Port_Id, ne.Device_MAC, ne.Event_Id));
        }
    }
}
