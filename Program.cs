using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace EventHubProcessingNet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //take details from built-in endpoint
            var eventHubName = "try-out-hub";
            var iotHubConnectionString = "Endpoint=sb://iothub-ns-try-out-hu-1005128-51185d9614.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=Z4VpahN/bHeRNaFp03ymXZU8L1ytMK3RD4gF3WWEbmE=;EntityPath=try-out-hub";

            // take the connection string from storage created
            var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=temperaturehubstorage;AccountKey=ZTcIUsl3/Iv+h6TouT1LtSSy76/vWfQar889GTGkhjRo1Z/mJbobA1+5rfZc3HYPglqIAG1qXwWBfulxSvLdcw==;EndpointSuffix=core.windows.net";
            var storageContainerName = "device-to-cloud-events";

            // imported from eventHub processor namespace
            var consumerGroupName = PartitionReceiver.DefaultConsumerGroupName;

            var processor = new EventProcessorHost(
            eventHubName,
            consumerGroupName,
            iotHubConnectionString,
            storageConnectionString,
            storageContainerName);


            
            await processor.RegisterEventProcessorAsync<LoggingEventProcessor>();

            Console.WriteLine("Event processor started, press enter to exit...");


            Console.ReadLine();

            await processor.UnregisterEventProcessorAsync();

        }
    }
}
