using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private DeviceClient deviceClient;

        public IoTDevice()
        {      
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
        }

        public async Task SendDeviceToCloudMessagesAsync(string jsonMessage)
        {


            var message = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            await deviceClient.SendEventAsync(message);
            System.Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, jsonMessage);
        }
    }
}
