using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UniSys.Messaging
{
    // Background service that continuously listens for messages from RabbitMQ.
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqConsumerService> _logger;

        // RabbitMQ Connection (TCP connection to the broker)
        private IConnection? _connection;

        // RabbitMQ Channel (virtual connection used for all RabbitMQ operations)
        private IChannel? _channel;

        public RabbitMqConsumerService(
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitMqConsumerService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // ConnectionFactory stores the information needed to connect to RabbitMQ.
                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    Port = _options.Port,
                    UserName = _options.UserName,
                    Password = _options.Password,
                };

                // Create the TCP connection to RabbitMQ.
                _connection = await factory.CreateConnectionAsync(stoppingToken);

                // Create a channel inside the connection.
                // All publishing and consuming happens through a channel.
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                // Create (or reuse) a Topic Exchange.
                // Exchanges receive messages from producers and route them to queues.
                await _channel.ExchangeDeclareAsync(
                    _options.Exchange,
                    ExchangeType.Topic,
                    durable: true,      // Survives RabbitMQ restart
                    cancellationToken: stoppingToken);

                // Create (or reuse) the queue that stores incoming messages.
                await _channel.QueueDeclareAsync(
                    _options.Queue,
                    durable: true,      // Queue survives restart
                    exclusive: false,  // Can be accessed by multiple connections
                    autoDelete: false, // Don't delete automatically
                    cancellationToken: stoppingToken);

                // Bind the queue to the exchange using a routing key.
                // Only matching messages will be delivered to this queue.
                await _channel.QueueBindAsync(
                    _options.Queue,
                    _options.Exchange,
                    _options.RoutingKey,
                    cancellationToken: stoppingToken);

                // Consumer object that listens for incoming messages.
                var consumer = new AsyncEventingBasicConsumer(_channel);

                // Triggered every time a message arrives in the queue.
                consumer.ReceivedAsync += (sender, args) =>
                {
                    // Convert the message body (bytes) into readable text.
                    var body = Encoding.UTF8.GetString(args.Body.ToArray());

                    // args.RoutingKey tells us which routing key delivered this message.
                    _logger.LogInformation(
                        "Received message [{RoutingKey}]: {Body}",
                        args.RoutingKey,
                        body);

                    // autoAck=true means RabbitMQ already considers this message processed.
                    return Task.CompletedTask;
                };

                // Start consuming messages from the queue.
                // autoAck=true means messages are automatically acknowledged.
                await _channel.BasicConsumeAsync(
                    _options.Queue,
                    autoAck: true,
                    consumer,
                    stoppingToken);

                _logger.LogInformation(
                    "Listening for messages on queue '{Queue}'.",
                    _options.Queue);
            }
            catch (Exception ex)
            {
                // If RabbitMQ is unavailable, log the error instead of crashing the application.
                _logger.LogWarning(ex, "Could not start the RabbitMQ consumer.");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Close the channel.
            if (_channel != null)
                await _channel.DisposeAsync();

            // Close the connection.
            if (_connection != null)
                await _connection.DisposeAsync();

            await base.StopAsync(cancellationToken);
        }
    }
}