//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portnox.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NetworkEvent
    {
        public int Event_Id { get; set; }
        public string Switch_Ip { get; set; }
        public byte Port_Id { get; set; }
        public string Device_MAC { get; set; }
    }
}