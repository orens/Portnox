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
    public class ScannerService : IScanner
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
        Dictionary<string, ISwitchPort> SwitchPorts = new Dictionary<string, ISwitchPort>();
        Dictionary<string, IDevice> Devices = new Dictionary<string, IDevice>();

        private IEnumerable<string> Switches1 => db.NetworkEvents.Select(s => s.Switch_Ip).Distinct();
    
        public async Task<IEnumerable<ISwitch>> GetSwitches()
        {
            try
            {
                Init();
                var switches = new List<ISwitch>();
                foreach (var currSwitchIp in Switches)
                {
                    var currSwitch = new Switch { Switch_Ip = currSwitchIp };
                    await PopulateSwitchAsync(currSwitch);
                    switches.Add(currSwitch);
                }
                return switches;
            }
            finally
            {
                db.Dispose();
            }
        }

        private void Init()
        {
            foreach (var item in db.NetworkEvents)
            {
                var event1 = new Event();
                event1.Event_Id = item.Event_Id;
                var device = GetDevice(item, event1);
                if (Sw)
                {

                }
            }
        }

        private IDevice GetDevice(NetworkEvent item, IEvent event1)
        {
            if (!Devices.ContainsKey(item.Device_MAC))
            {
                Devices.Add(item.Device_MAC, new Device(item.Device_MAC));
            }
            return Devices[item.Device_MAC];
        }

        private async Task PopulateSwitchAsync(ISwitch currSwitch)
        {
            currSwitch.Ports = await GetPortsAsync(currSwitch);
            currSwitch.Events = await GetEventsAsync(currSwitch);
        }
        public async Task<IEnumerable<ISwitchPort>> GetPortsAsync(ISwitch currSwitch)
        {

            var ports = await db.NetworkEvents.Where(s => s.Switch_Ip == currSwitch.Switch_Ip).Select(s =>
            new SwitchPort
            {
                Port_Id = s.Port_Id,
            }).Distinct().ToListAsync();

            foreach (var port in ports)
            {
                port.Devices = await GetDevicesAsync(currSwitch, port.Port_Id);
                port.Events = await GetPortEventsAsync(currSwitch, port.Port_Id);
            }
            return ports;
        }

        public async Task<IEnumerable<IEvent>> GetEventsAsync(ISwitch currSwitch)
        {
            return await db.NetworkEvents.Where(s => s.Switch_Ip == currSwitch.Switch_Ip)
                .Select(s => new Event
                {
                    Event_Id = s.Event_Id
                }).ToListAsync();
        }

        public async Task<IEnumerable<IDevice>> GetDevicesAsync(ISwitch currSwitch, byte port_Id)
        {
           var devices =  await db.NetworkEvents.Where(s => s.Switch_Ip == currSwitch.Switch_Ip && s.Port_Id == port_Id).Select(s =>
           new Device
           {
               Device_MAC = s.Device_MAC,
           }).Distinct().ToListAsync();

            foreach (var device in devices)
            {
                device.Events = await GetPortEventsAsync(currSwitch, port_Id, device.Device_MAC);
            }
            return devices;
        }

        public async Task<IEnumerable<IEvent>> GetPortEventsAsync(ISwitch currSwitch, byte port_Id, string device_MAC = null)
        {
            var query = db.NetworkEvents.Where(s => s.Switch_Ip == currSwitch.Switch_Ip && s.Port_Id == port_Id);
            if (device_MAC != null)
            {
                query = query.Where(s => s.Device_MAC == device_MAC);
            }
            return await query.Select(s => new Event{Event_Id = s.Event_Id}).ToListAsync();
        }
    }
}
