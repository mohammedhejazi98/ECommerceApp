using RabbitMQ.Client;

namespace OrderService.Common.Constants.RabbitMq
{
    public static class RabbitMqConnection
    {
        private static IConnection _connection;
        private static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
            Initialize();
        }

        public static IConnection GetConnection()
        {
            if (_connection == null)
                Initialize();
            return _connection;
        }
        public static void Initialize()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5673,
                UserName = "guest",
                Password = "guest"

            };
            _connection = factory.CreateConnection();
        }
    }
}
