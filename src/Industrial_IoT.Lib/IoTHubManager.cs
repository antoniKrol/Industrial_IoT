using Microsoft.Azure.Devices;
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

    }
}