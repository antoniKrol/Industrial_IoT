using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;

namespace Industrial_IoT.Lib
{
    public class IoTHubManager
    {
        private readonly ServiceClient client;
        private readonly Dictionary<string, DeviceClient> deviceClients;
        public IoTHubManager(ServiceClient client, IConfiguration configuration)
        {
            this.client = client;

            this.deviceClients = new Dictionary<string, DeviceClient>();
            var deviceConnectionStrings = configuration.GetSection("deviceConnectionString")
                .GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var kvp in deviceConnectionStrings)
            {
                string deviceId = kvp.Key;
                string deviceConnectionString = kvp.Value;
                var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString);
                this.deviceClients.Add(deviceId, deviceClient);
            }
        }

        public async Task SendMessage(string textMessage,string deviceId)
        {
            var messageBody = new { text = textMessage };
            var message = new Microsoft.Azure.Devices.Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody)));
            message.MessageId = Guid.NewGuid().ToString();
            await client.SendAsync(deviceId,message);
        }

        public async Task<int> ExecuteDeviceMethod(string methodName,string deviceId)
        {
            var method=new CloudToDeviceMethod(methodName);
            var methodBody = new { };
            method.SetPayloadJson(JsonConvert.SerializeObject(methodBody));

            
            try
            {
               var result = await client.InvokeDeviceMethodAsync(deviceId, method);
                return result.Status;
            }
            catch (DeviceNotFoundException)
            {
                System.Console.WriteLine("Device Not Found!!!");
                System.Console.ReadLine();
                return 404;
            }
           
        }

        public async Task UpdateReportedDeviceTwin(string deviceId, int deviceError, int productionRate)
        {
            // Check if the deviceClient for the given deviceId exists in the dictionary
            if (this.deviceClients.TryGetValue(deviceId, out var deviceClient))
            {
                var twinProperties = new TwinCollection();
                twinProperties["DeviceError"] = deviceError;
                twinProperties["ProductionRate"] = productionRate;

                // Update the reported properties of the specific device
                await deviceClient.UpdateReportedPropertiesAsync(twinProperties);
            }
            else
            {
                throw new Exception($"Device with id {deviceId} not found in deviceClients dictionary.");
            }
        }
    }
}