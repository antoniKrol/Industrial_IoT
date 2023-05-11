using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class DeviceData
    {
        public string DeviceName { get; set; }
        public string ProductionStatus { get; set; }
        public string WorkOrderId { get; set; }
        public string Temperature { get; set; }
        public string GoodCount { get; set; }
        public string BadCount { get; set; }
    }
}
