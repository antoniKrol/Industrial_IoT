using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace Industrial_IoT.Lib
{
    public class IoTHubManager
    {
        private readonly ServiceClient client;
        public IoTHubManager(ServiceClient client)
        {
            this.client = client;
        }

        public async Task SendMessage(string textMessage,string deviceId)
        {
            var messageBody = new { text = textMessage };
            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody)));
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
    }
}