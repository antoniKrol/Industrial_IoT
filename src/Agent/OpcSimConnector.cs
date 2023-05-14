using Microsoft.Azure.Devices;
using Opc.UaFx.Client;
using Opc.UaFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Devices.Shared;

namespace Agent.Console
{
    internal class OpcSimConnector
    {
        private readonly string opcClientConnetionString = "";
        private readonly Industrial_IoT.Lib.IoTHubManager manager;

        public OpcSimConnector(Industrial_IoT.Lib.IoTHubManager manager)
        {
            IConfiguration configuration = AppConfiguration.GetConfiguration();
            opcClientConnetionString = configuration["opcClientConnetionString"];
            this.manager = manager;
        }

        public async void connectAndDisplay()
        {
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

                        //Direct Method listener
                        foreach (var device in deviceNode.Children())
                        {
                            if (device.Name.ToString().StartsWith("Device"))
                            {
                                string deviceIdS = device.Name.ToString().Replace(" ", "");
                                manager.RegisterDirectMethodListener(deviceIdS);
                            }
                        }

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

                                string deviceIdS = deviceData.DeviceId.Replace(" ", "");

                                string jsonMessage = JsonConvert.SerializeObject(deviceData);

                                int deviceError = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/DeviceError")).As<int>();
                                int productionRate = client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/ProductionRate")).As<int>();
                                await manager.SendDeviceToCloudMessageAsync(deviceIdS, jsonMessage);

                                if(await manager.UpdateReportedDeviceTwin(deviceIdS, deviceError, productionRate)){

                                    var msg = new EventMessage
                                    {
                                        DeviceId = deviceIdS,
                                        Type = "NewError",
                                        DeviceError = deviceError
                                    };
                                    await manager.SendDeviceToCloudMessageAsync(deviceIdS, JsonConvert.SerializeObject(msg));
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
