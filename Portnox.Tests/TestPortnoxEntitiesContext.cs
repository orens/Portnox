using Portnox.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Tests
{
    class TestPortnoxEntitiesContext : DbContext
    {
        public DbSet<NetworkEvent> NetworkEvents { get; set; }

    }
}
