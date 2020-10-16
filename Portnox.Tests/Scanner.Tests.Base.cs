using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portnox.DataLayer;
using Portnox.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Portnox.Tests
{
    public class TestScannerBase
    {
        protected PortnoxEntities context;
        IQueryable<NetworkEvent> data;

        public TestScannerBase(IList<NetworkEvent> baseData)
        {
            data = baseData.AsQueryable();
        }
        [TestInitialize]
        public void Initialize()
        {

            var mockSet = new Mock<DbSet<NetworkEvent>>();
            mockSet.As<IDbAsyncEnumerable<NetworkEvent>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<NetworkEvent>(data.GetEnumerator()));

            mockSet.As<IQueryable<NetworkEvent>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<NetworkEvent>(data.Provider));

            mockSet.As<IQueryable<NetworkEvent>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<NetworkEvent>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<NetworkEvent>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<PortnoxEntities>();
            mockContext.Setup(c => c.NetworkEvents).Returns(mockSet.Object);
            context = mockContext.Object;
        }

        protected static List<NetworkEvent> RandomizeData()
        {
            var returnValue = new List<NetworkEvent>();
            var rand = new Random();
            var switchCount = new int[rand.Next(0, 50)];
            foreach (var @switch in switchCount)
            {
                returnValue.Add(new NetworkEvent { Switch_Ip = SelectSwitch(rand), Port_Id = (byte)rand.Next(1, 48), Event_Id = SelectEvent(rand), Device_MAC = SelectMAC(rand) });
            }

            return returnValue;
        }

        private static string SelectMAC(Random rand)
        {
            var macs = new[] {null, "00112233445566", "00112233445511", "001122333112233", "009988776655" };
            return macs[rand.Next(0, macs.Length - 1)];
        }

        private static int SelectEvent(Random rand)
        {
            var events = new[] { 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010 };
            return events[rand.Next(0, events.Length - 1)];
        }

        private static string SelectSwitch(Random rand)
        {
            var switches = new[] { "1.1.1.1", "1.1.1.2", "192.168.10.1", "192.168.1.1" };
            return switches[rand.Next(0, switches.Length - 1)];
        }
    }
}
