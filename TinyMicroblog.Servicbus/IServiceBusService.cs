namespace TinyMicroblog.Servicebus.Interfaces
{
    public interface IServiceBusService
    {
        Task SendScheduledNotification(string queueName, object serviceBusMessageModel, DateTimeOffset time);
        Task SendNotification(string queueName, object serviceBusMessageModel);
        Task CancelScheduleNotification(long sequenceNumber, string queueName);
    }
}
