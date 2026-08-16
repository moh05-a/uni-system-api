namespace UniSys.Messaging
{
    // Sends a message to RabbitMQ as JSON.
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string routingKey, T message);
    }
}
