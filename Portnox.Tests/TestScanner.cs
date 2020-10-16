using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portnox.DataLayer;
using Portnox.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Tests
{
    [TestClass]
    public class TestScanner : TestScannerBase
    {
        public TestScanner() : base(RandomizeData())
        { }

        [TestMethod]
        public void SwitchesCount()
        {
            var scanner = new ScannerService(context);
            var switches = scanner.GetAllSwitches();

            Assert.AreEqual(context.NetworkEvents.Select(s => s.Switch_Ip).Distinct().Count(), switches.Count());
        }

        [TestMethod]
        public void SwitchesEmpty()
        {
            context.NetworkEvents.RemoveRange(context.NetworkEvents);
            context.SaveChanges();
            var scanner = new ScannerService(context);
            var switches = scanner.GetAllSwitches();

            Assert.AreEqual(context.NetworkEvents.Select(s => s.Switch_Ip).Distinct().Count(), switches.Count());
        }

        [TestMethod]
        public void PortPerSwitchCount()
        {
            var scanner = new ScannerService(context);
            var switches = scanner.GetAllSwitches();

            foreach (var @switch in switches)
            {
                Assert.AreEqual(context.NetworkEvents.Where(f => f.Switch_Ip == @switch.Switch_Ip).Select(p => p.Port_Id).Distinct().Count(), @switch.Ports.Count());
            }
        }
        
        [TestMethod]
        public void NUmberOfEventsInOneSwitch()
        {
            var scanner = new ScannerService(context);
            var switches = scanner.GetAllSwitches();

            var firstSwitch = switches.First();

            Assert.AreEqual(context.NetworkEvents.Where(f => f.Switch_Ip == firstSwitch.Switch_Ip).Select(p => p.Event_Id).Count(), firstSwitch.Events.Count());
        }
    }
}


