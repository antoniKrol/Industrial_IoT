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

        public bool CheckAndUpdateLocalDeviceTwin(string deviceId, string propertyName, int propertyValue)
        {
            // Define the directory for the device twin files.
            string directoryPath = "DeviceTwins";

            // Ensure the directory exists.
            Directory.CreateDirectory(directoryPath);

            // Construct the file path for the device twin.
            string filePath = Path.Combine(directoryPath, $"{deviceId}.json");

            JObject deviceTwin;

            // If the file doesn't exist, create a new JObject with the necessary structure.
            if (!File.Exists(filePath))
            {
                deviceTwin = new JObject(
                    new JProperty("deviceId", deviceId),
                    new JProperty("properties",
                        new JObject(
                            new JProperty("desired", new JObject()),
                            new JProperty("reported", new JObject())
                        )
                    )
                );
            }
            // If the file exists, read the file content and deserialize it to a JObject.
            else
            {
                string fileContent = File.ReadAllText(filePath);
                deviceTwin = JObject.Parse(fileContent);
            }

            // Check if the property value has changed.
            JToken currentPropertyValue = deviceTwin["properties"]["reported"][propertyName];
            if (currentPropertyValue == null || (int)currentPropertyValue != propertyValue)
            {
                // If the property value has changed, update it in the reported properties.
                deviceTwin["properties"]["reported"][propertyName] = propertyValue;

                // Serialize the JObject back to a JSON string and write it to the file.
                string updatedFileContent = deviceTwin.ToString();
                File.WriteAllText(filePath, updatedFileContent);

                // Return true to indicate that the property value has changed.
                return true;
            }
            else
            {
                // If the property value has not changed, return false.
                return false;
            }
        }
    }
}
