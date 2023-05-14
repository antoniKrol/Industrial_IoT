using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class DeviceData
    {
        public string DeviceId { get; set; }
        public int ProductionStatus { get; set; }
        public string WorkOrderId { get; set; }
        public double Temperature { get; set; }
        public int GoodCount { get; set; }
        public int BadCount { get; set; }
    }
    internal class EventMessagr
    {
        public string DeviceId { get; set; }
        public string Type { get; set; }
        public int DeviceError { get;set; }
    }
}
