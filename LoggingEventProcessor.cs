using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventHubProcessingNet
{
    class LoggingEventProcessor : IEventProcessor

    {

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("LoggingEventProcessor closing, partition: " +
                              $"'{context.PartitionId}', reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine("LoggingEventProcessor opened, processing partition: " +
                              $"'{context.PartitionId}'");
           
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine("LoggingEventProcessor error, partition: " +
                              $"{context.PartitionId}, error: {error.Message}");
            return Task.CompletedTask;
        }

        public  Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            Console.WriteLine($"Batch of events received on partition '{context.PartitionId}'.");
            foreach (var eventData in messages)
            {
                
                var payload = Encoding.ASCII.GetString(eventData.Body.Array,
                    eventData.Body.Offset,
                    eventData.Body.Count);
                
                
                //cosmos db can only send objects
                var message = JsonConvert.DeserializeObject<MessageBody>(payload);
                Console.WriteLine($"Payload sent to DB is '{payload}'.");

                CosmosDB db = new CosmosDB();
                db.Init().Wait();
                db.pushTemperatureDocument(message).Wait();
                //nullify created object
                db = null;
                Console.WriteLine("The object destroyed");
               
            }
            return context.CheckpointAsync();
        }
    }
}