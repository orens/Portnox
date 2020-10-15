using Portnox.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox
{
    class Program
    {
        static void Main(string[] args)
        {
            var scanner = new ScannerService();
            var result = scanner.GetSwitches();
        }
    }
}
