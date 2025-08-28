namespace RustRetail.CatalogService.Infrastructure.MessageBrokers.RabbitMQ
{
    public class RabbitMQOptions
    {
        public const string SectionName = "RabbitMQ";

        public string Host { get; set; } = "localhost";
        public string VirtualHost { get; set; } = "/";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string ExchangeName { get; set; } = "RustRetail.CatalogService";
    }
}
