using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace UniSys.Messaging
{
    // Publisher service that sends messages to RabbitMQ.
    // Registered as a Singleton so one connection/channel is reused.
    public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
    {
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqPublisher> _logger;

        // RabbitMQ Connection (TCP connection to the broker)
        private IConnection? _connection;

        // RabbitMQ Channel (virtual connection used for publishing)
        private IChannel? _channel;

        public RabbitMqPublisher(
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitMqPublisher> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        // Publishes a message to RabbitMQ.
        public async Task PublishAsync<T>(string routingKey, T message)
        {
            try
            {
                // Connect to RabbitMQ if a connection/channel doesn't already exist.
                if (_channel == null)
                    await ConnectAsync();

                // Convert the object into JSON bytes before sending.
                var body = JsonSerializer.SerializeToUtf8Bytes(message);

                // Publish the message to the exchange.
                // The exchange uses the routing key to decide which queue(s) receive it.
                await _channel!.BasicPublishAsync(
                    _options.Exchange,   // Exchange that receives the message
                    routingKey,          // Routing Key used for message routing
                    body);               // Message payload

                _logger.LogInformation(
                    "Published message with routing key '{RoutingKey}'.",
                    routingKey);
            }
            catch (Exception ex)
            {
                // If RabbitMQ is unavailable, don't crash the API.
                // Simply log the error and continue serving requests.
                _logger.LogWarning(
                    ex,
                    "Could not publish message with routing key '{RoutingKey}'.",
                    routingKey);
            }
        }

        // Creates the RabbitMQ connection and channel.
        private async Task ConnectAsync()
        {
            // ConnectionFactory stores the broker connection settings.
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
            };

            // Create the TCP connection to RabbitMQ.
            _connection = await factory.CreateConnectionAsync();

            // Create a channel used for publishing messages.
            _channel = await _connection.CreateChannelAsync();

            // Create (or reuse) the Topic Exchange.
            // Producers publish to exchanges, not directly to queues.
            await _channel.ExchangeDeclareAsync(
                _options.Exchange,
                ExchangeType.Topic,
                durable: true); // Exchange survives RabbitMQ restart

            _logger.LogInformation(
                "Connected to RabbitMQ at {Host}:{Port}.",
                _options.HostName,
                _options.Port);
        }

        public async ValueTask DisposeAsync()
        {
            // Close the channel.
            if (_channel != null)
                await _channel.DisposeAsync();

            // Close the connection.
            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}