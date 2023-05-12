using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class ServiceBusKpiAlerts
    {
        [FunctionName("ServiceBusKpiAlerts")]
        public static void Run([ServiceBusTrigger("%ServiceBusQueueName%", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            Console.WriteLine("SADASDAS");
        }
    }
}
