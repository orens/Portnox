﻿using Portnox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portnox.Models
{
    public class Switch : ISwitch
    {
        public string Switch_Ip { get; set; }
        public IEnumerable<ISwitchPort> Ports { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}