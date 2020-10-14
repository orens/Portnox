using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portnox.DataLayer;
using Portnox.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Portnox.Tests
{
    [TestClass]
    public class TestScanner
    {
        private PortnoxEntities context;
        [TestInitialize]
        public void Initialize()
        {
            var data = new List<NetworkEvent>
            {
                new NetworkEvent{ Event_Id = 1001, Switch_Ip = "1.1.1.1", Port_Id = 12, Device_MAC = "AABBCC000001" },
                new NetworkEvent { Event_Id = 1001, Switch_Ip = "1.1.1.1", Port_Id = 11, Device_MAC = "AABBCC000009" },
                new NetworkEvent { Event_Id = 1003, Switch_Ip = "192.168.1.1", Port_Id = 48, Device_MAC = null },
                new NetworkEvent { Event_Id = 1002, Switch_Ip = "1.1.1.1", Port_Id = 12, Device_MAC = null },
                new NetworkEvent { Event_Id = 1001, Switch_Ip = "192.168.1.1", Port_Id = 47, Device_MAC = "AABBCC000001" },
                new NetworkEvent { Event_Id = 1001, Switch_Ip = "192.168.1.1", Port_Id = 47, Device_MAC = "AABBCC000001" },
                new NetworkEvent { Event_Id = 1001, Switch_Ip = "192.168.1.1", Port_Id = 47, Device_MAC = "AABBCC000001" },
                new NetworkEvent { Event_Id = 1001, Switch_Ip = "192.168.1.1", Port_Id = 47, Device_MAC = "AABBCC000001" },
                new NetworkEvent { Event_Id = 1001, Switch_Ip = "192.168.1.1", Port_Id = 47, Device_MAC = "AABBCC000001" },

            }.AsQueryable();

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

        [TestMethod]
        public void GetSwitches_ShouldReturn_2_Switches()
        {
            var scanner = new ScannerService(context);
            var switches = scanner.GetSwitches();

            Assert.AreEqual(2, switches.Result.Count());
        }

        [TestMethod]
        public void GetPorts_ShouldGet_3()
        {
            var scanner = new ScannerService(context);
            var ports = scanner.GetPortsAsync("1.1.1.1");

            Assert.AreEqual(3, ports.Result.Count());
        }
    }
}
