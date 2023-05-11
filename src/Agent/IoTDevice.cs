using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class IoTDevice
    {
       
        string deviceConnectionString = "HostName=Zajecia-IOT.azure-devices.net;DeviceId=Industrial_IoT;SharedAccessKey=9fVTEZ1Bd+3AaxvU/tvLTL+02KnCJr3YIRPTU2srawk=";

        public async Task SendDeviceToCloudMessagesAsync()
        {

            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);

            var deviceData = new
            {
                // Add your device data here. For example:
                deviceId = "myFirstDevice",
                windSpeed = 10.0,
                temperature = 20.0
                // Add other fields as necessary.
            };

            var messageString = JsonConvert.SerializeObject(deviceData);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
            System.Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

            await Task.Delay(1);
        }
    }
}
