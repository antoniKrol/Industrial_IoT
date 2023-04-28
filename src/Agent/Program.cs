using DeviceSdkAgent.Device;
using Microsoft.Azure.Devices.Client;

string deviceConnectionString = "HostName=Zajecia-IOT.azure-devices.net;DeviceId=Industrial_IoT;SharedAccessKey=9fVTEZ1Bd+3AaxvU/tvLTL+02KnCJr3YIRPTU2srawk=";

using var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);

await deviceClient.OpenAsync();

var device = new VirtualDevice(deviceClient);

Console.WriteLine("Connected");