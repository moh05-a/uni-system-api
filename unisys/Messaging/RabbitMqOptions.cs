namespace UniSys.Messaging
{
    public class RabbitMqOptions
    {
        public const string SectionName = "RabbitMQ";

        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";

        // Exchange we publish events to.
        public string Exchange { get; set; } = "unisys.events";

        // Queue the consumer listens on.
        public string Queue { get; set; } = "unisys.queue";

        // Which messages the queue receives, e.g. "student.#" means every student event.
        public string RoutingKey { get; set; } = "student.#";
    }
}
