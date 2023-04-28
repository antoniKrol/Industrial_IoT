using DeviceSdkAgent.Device;
using Microsoft.Azure.Devices;
using Industrial_IoT.Lib;
using Agent.Console;
using Opc.UaFx.Client;
using Opc.UaFx;

string opcClientConnetionString = "opc.tcp://localhost:4840/";

using (var client = new OpcClient(opcClientConnetionString))
{
    client.Connect();

    OpcNodeInfo deviceNode = client.BrowseNode("i=85");
    foreach (var device in deviceNode.Children())
    {
        if (device.Name.ToString().StartsWith("Device"))
        {
            Console.WriteLine($"\n{device.Name}");

            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/ProductionStatus",OpcAttribute.DisplayName)));
            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/ProductionStatus")));

            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/ProductionRate", OpcAttribute.DisplayName)));
            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/ProductionRate")));

            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/WorkorderId", OpcAttribute.DisplayName)));
            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/WorkorderId")));

            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/Temperature", OpcAttribute.DisplayName)));
            Console.WriteLine(client.ReadNode(new OpcReadNode($"ns=2;s={device.Name}/Temperature")));
        }
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
