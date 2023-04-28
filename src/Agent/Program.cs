using DeviceSdkAgent.Device;
using Microsoft.Azure.Devices;
using Industrial_IoT.Lib;
using Agent.Console;

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
