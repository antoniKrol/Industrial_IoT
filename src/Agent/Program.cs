using DeviceSdkAgent.Device;
using Microsoft.Azure.Devices;
using Industrial_IoT.Lib;
using Agent.Console;
using Opc.UaFx.Client;
using Opc.UaFx;

string opcClientConnetionString = "opc.tcp://localhost:4840/";

using (var client = new OpcClient(opcClientConnetionString))
{
    try
    {
    client.Connect();

        while (true)
        {
            Console.Clear();
            OpcNodeInfo deviceNode = client.BrowseNode("i=85");
            foreach (var device in deviceNode.Children())
            {
                if (device.Name.ToString().StartsWith("Device"))
                {
                    Console.WriteLine($"\n{device.Name}");

                    var productionStatus = new OpcReadNode($"ns=2;s={device.Name}/ProductionStatus");
                    var productionRate = new OpcReadNode($"ns=2;s={device.Name}/ProductionRate");
                    var workOrderId = new OpcReadNode($"ns=2;s={device.Name}/WorkorderId");
                    var temperature = new OpcReadNode($"ns=2;s={device.Name}/Temperature");
                    var goodCount = new OpcReadNode($"ns=2;s={device.Name}/GoodCount");
                    var badCount = new OpcReadNode($"ns=2;s={device.Name}/BadCount");
                    var deviceError = new OpcReadNode($"ns=2;s={device.Name}/DeviceError");


                    Console.WriteLine(client.ReadNode(productionStatus.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(productionStatus));

                    Console.WriteLine(client.ReadNode(productionRate.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(productionRate));

                    Console.WriteLine(client.ReadNode(workOrderId.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(workOrderId));

                    Console.WriteLine(client.ReadNode(temperature.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(temperature));

                    Console.WriteLine(client.ReadNode(goodCount.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(goodCount));

                    Console.WriteLine(client.ReadNode(badCount.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(badCount));

                    Console.WriteLine(client.ReadNode(deviceError.NodeId, OpcAttribute.DisplayName));
                    Console.WriteLine(client.ReadNode(deviceError));
                }
            }
                Thread.Sleep(1000);
        }
    }
    catch(OpcException ex)
    {
        Console.WriteLine(ex);
    }

   
}

Console.ReadLine();
string serviceConnectionString = "HostName=Zajecia-IOT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=y3rf6CmkGHtDggko9oHrC5Xtyf3FDjcrHVSypUwn/4w=";

using var serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

var manager = new IoTHubManager(serviceClient);

int input;
do
{
    System.Console.Clear();
    FeatureSelector.PrintMenu();
    input = FeatureSelector.ReadInput();
    await FeatureSelector.Execute(input, manager);

}while(input != 0);

Console.WriteLine("Done");
