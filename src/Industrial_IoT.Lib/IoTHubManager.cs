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
                string deviceConnectionString = kvp.Value!;
                var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString);
                this.deviceClients.Add(deviceId, deviceClient);
            }
        }

        public async Task SendMessage(string textMessage,string deviceId)
        {
            var messageBody = new { text = textMessage };
            var message = new Microsoft.Azure.Devices.Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody)));
            message.MessageId = Guid.NewGuid().ToString();
            try
            {
                await client.SendAsync(deviceId, message);
            }catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public async Task SendDeviceToCloudMessageAsync(string deviceId, string textMessage)
        {
            if (!this.deviceClients.TryGetValue(deviceId, out DeviceClient deviceClient))
            {
                throw new Exception($"Device with id {deviceId} not found in deviceClients dictionary.");
            }

            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(textMessage))
            {
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            try
            {
                await deviceClient.SendEventAsync(message);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
        public void RegisterDirectMethodListener(string deviceId)
        {
            if (!this.deviceClients.TryGetValue(deviceId, out DeviceClient deviceClient))
            {
                throw new Exception($"Device with id {deviceId} not found in deviceClients dictionary.");
            }

            deviceClient.SetMethodDefaultHandlerAsync((MethodRequest request, object userContext) =>
            {
                System.Console.WriteLine($"Direct method invoked: {request.Name}");
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { result = "Executed " + request.Name })), 200));
            }, null);
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

        public async Task<bool> UpdateReportedDeviceTwin(string deviceId, int deviceError, int productionRate)
        {
            if (!this.deviceClients.TryGetValue(deviceId, out DeviceClient deviceClient))
            {
                throw new Exception($"Device with id {deviceId} not found in deviceClients dictionary.");
            }

            // Retrieve the existing device twin
            var twin = await deviceClient.GetTwinAsync();
            var lastReportedDeviceError = twin.Properties.Reported.Contains("DeviceError") ? twin.Properties.Reported["DeviceError"] : null;

            // Compare with the new deviceError
            if (lastReportedDeviceError != null && (int)lastReportedDeviceError == deviceError)
            {
                var twinProperties = new TwinCollection();
                twinProperties["DeviceError"] = deviceError;
                twinProperties["ProductionRate"] = productionRate;
                await deviceClient.UpdateReportedPropertiesAsync(twinProperties);
                return false;
            }
            else
            {
                // Update the device twin
                var twinProperties = new TwinCollection();
                twinProperties["DeviceError"] = deviceError;
                twinProperties["ProductionRate"] = productionRate;
                await deviceClient.UpdateReportedPropertiesAsync(twinProperties);
                return true;
            }
        }
    }
}