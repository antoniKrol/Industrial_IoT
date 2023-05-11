using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class DeviceTwin
    {
        public string DeviceId { get; set; }
        public Dictionary<string, object> ReportedProperties { get; set; }
        public Dictionary<string, object> DesiredProperties { get; set; }

        public DeviceTwin(string deviceId) {
            DeviceId = deviceId;
            ReportedProperties = new Dictionary<string, object>();
            DesiredProperties = new Dictionary<string, object>();
        }
    }
}
