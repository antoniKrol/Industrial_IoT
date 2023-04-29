using Microsoft.Azure.Devices;
using Opc.UaFx.Client;
using Opc.UaFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class OpcSimConnector
    {
        private readonly string opcClientConnetionString = "opc.tcp://localhost:4840/";

        public void connectAndDisplay()
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
                        foreach (var device in deviceNode.Children())
                        {
                            if (device.Name.ToString().StartsWith("Device"))
                            {
                                System.Console.WriteLine($"\n{device.Name}");

                                var productionStatus = new OpcReadNode($"ns=2;s={device.Name}/ProductionStatus");
                                var productionRate = new OpcReadNode($"ns=2;s={device.Name}/ProductionRate");
                                var workOrderId = new OpcReadNode($"ns=2;s={device.Name}/WorkorderId");
                                var temperature = new OpcReadNode($"ns=2;s={device.Name}/Temperature");
                                var goodCount = new OpcReadNode($"ns=2;s={device.Name}/GoodCount");
                                var badCount = new OpcReadNode($"ns=2;s={device.Name}/BadCount");
                                var deviceError = new OpcReadNode($"ns=2;s={device.Name}/DeviceError");


                                System.Console.WriteLine(client.ReadNode(productionStatus.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(productionStatus));

                                System.Console.WriteLine(client.ReadNode(productionRate.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(productionRate));

                                System.Console.WriteLine(client.ReadNode(workOrderId.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(workOrderId));

                                System.Console.WriteLine(client.ReadNode(temperature.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(temperature));

                                System.Console.WriteLine(client.ReadNode(goodCount.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(goodCount));

                                System.Console.WriteLine(client.ReadNode(badCount.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(badCount));

                                System.Console.WriteLine(client.ReadNode(deviceError.NodeId, OpcAttribute.DisplayName));
                                System.Console.WriteLine(client.ReadNode(deviceError));
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
