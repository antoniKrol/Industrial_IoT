using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace IoTAzure.Func
{
    public class KpiAlerts
    {
        [FunctionName("KpiAlerts")]
        public void Run([ServiceBusTrigger("%ServiceBusQueuename%", Connection = "ServiceBusConnectionsString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            Console.WriteLine(myQueueItem);
        }
    }
}
