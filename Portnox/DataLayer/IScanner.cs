using Portnox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.DataLayer
{
    interface IScanner
    {
        Task<IEnumerable<ISwitch>> GetSwitches();
    }
}
