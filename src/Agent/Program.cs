using DeviceSdkAgent.Device;
using Microsoft.Azure.Devices;
using Industrial_IoT.Lib;
using Agent.Console;
using Microsoft.Extensions.Configuration;


// Create a ConfigurationBuilder
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Build the configuration
IConfiguration configuration = builder.Build();

// Get the connection string from the configuration
string serviceConnectionString = configuration["serviceConnectionString"];

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
