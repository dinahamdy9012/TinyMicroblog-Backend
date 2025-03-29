using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TinyMicroblog.Servicebus.Interfaces;

namespace TinyMicroblog.Servicebus.Services
{
   public class ServiceBusService: IServiceBusService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;

        public ServiceBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new ServiceBusClient(_configuration["ConnectionStrings:ServiceBus"]);
        }

        public async Task SendScheduledNotification(string queueName, object serviceBusMessageModel, DateTimeOffset time)
        {
            // Create a message to send
            var sender = _client.CreateSender(queueName);
            var serializedModel = JsonConvert.SerializeObject(serviceBusMessageModel);

            ServiceBusMessage message = new ServiceBusMessage(serializedModel);
            var sequenceNumber = await sender.ScheduleMessageAsync(message, time);
            Console.WriteLine($"Message scheduled to be sent at {time}");

        }

        public async Task SendNotification(string queueName, object serviceBusMessageModel)
        {
            ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(serviceBusMessageModel));
            var sender = _client.CreateSender(queueName);
            await sender.SendMessageAsync(message);
        }

        public async Task CancelScheduleNotification(long sequenceNumber, string queueName)
        {
            var sender = _client.CreateSender(queueName);
            await sender.CancelScheduledMessageAsync(sequenceNumber);
        }
    }
}
