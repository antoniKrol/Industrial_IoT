using Microsoft.Azure.Devices;
using Opc.UaFx.Client;
using Opc.UaFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Agent.Console
{
    internal class OpcSimConnector
    {
        private readonly string opcClientConnetionString = "opc.tcp://localhost:4840/";

        public async void connectAndDisplay()
        {
            IoTDevice IoTDevice = new IoTDevice();

            using (var client = new OpcClient(opcClientConnetionString))
            {
                try
                {
                   client.Connect();
                    bool continueLoop = true;
                    while (continueLoop)
                    {
                        System.Console.Clear();
                        OpcNodeInfo deviceNode = client.BrowseNode("i=85");
                        foreach (var device in deviceNode.Children())
                        {
                            if (device.Name.ToString().StartsWith("Device"))
                            {
                                System.Console.WriteLine($"\n{device.Name}");

                                var deviceData = new DeviceData
                                {
                                    DeviceId = device.Name.ToString(),
                                    ProductionStatus = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/ProductionStatus")).As<int>(),
                                    WorkOrderId = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/WorkorderId")).ToString(),
                                    Temperature = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/Temperature")).As<double>(),
                                    GoodCount = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/GoodCount")).As<int>(),
                                    BadCount = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/BadCount")).As<int>(),
                                };

                                string jsonMessage = JsonConvert.SerializeObject(deviceData);

                                int deviceError = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/DeviceError")).As<int>();

                                await IoTDevice.SendDeviceToCloudMessagesAsync(jsonMessage);
                                if(IoTDevice.CheckAndUpdateLocalDeviceTwin(deviceData.DeviceId,"DeviceError",deviceError))
                                {

                                    var messageObject = new
                                    {
                                        DeviceId = deviceData.DeviceId,
                                        DeviceErrors = deviceError
                                    };
                                    string json = JsonConvert.SerializeObject(messageObject);

                                    await IoTDevice.SendDeviceToCloudMessagesAsync(json);
                                }
                            } 
                        }
                        if(System.Console.KeyAvailable)
                        {
                            var key = System.Console.ReadKey(intercept: true);
                            if (key.KeyChar == '0')
                            {
                                continueLoop = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
                catch (OpcException ex)
                {
                    System.Console.WriteLine(ex);
                }

            }
        }

    }
}
