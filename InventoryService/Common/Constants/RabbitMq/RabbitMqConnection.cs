﻿using RabbitMQ.Client;

namespace InventoryService.Common.Constants.RabbitMq
{
    public static class RabbitMqConnection
    {
        private static IConnection? _connection;
        private static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
            Initialize();
        }

        public static IConnection? GetConnection()
        {
            if (_connection == null)
                Initialize();
            return _connection;
        }
        public static void Initialize()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHost"],
            };
            _connection = factory.CreateConnection();
        }
    }
}
