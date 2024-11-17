

// ReSharper disable LocalizableElement

using InventoryService.Common.Constants.RabbitMq;

using RabbitMQ.Client;

using System.Text;

namespace InventoryService.Services.ServiceBus
{
    public class MessagesServiceBus : IMessagesServiceBus
    {
        #region Fields

        private IModel? _channel;
        private IConnection? _connection;

        #endregion Fields

        #region Methods

        public void PublishToConsumerDb(string message, string exchangeName, string routingKey, Dictionary<string, object> headers)
        {
            //if (_connection is null)
            Connect(exchangeName);

            if (_connection is { IsOpen: true })
            {
                Console.WriteLine("RabbitMQ Connection Is Open");
                var result = SendMessage(message, exchangeName, routingKey, headers);
                _channel?.Close();
                if (result)
                    Console.WriteLine($"{message} has been sent");
            }
        }


        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            //_connection.Dispose();
            Console.WriteLine("Connection has been shutdown");
        }
        private void Connect(string exchangeName)
        {
            _connection = RabbitMqConnection.GetConnection();
            _channel = _connection?.CreateModel();
            _channel?.ConfirmSelect();
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
            if (_connection != null) _connection.ConnectionShutdown += _connection_ConnectionShutdown;
            Console.WriteLine("Connection has been created");
        }

        private bool SendMessage(string message, string exchangeName, string routingKey, Dictionary<string, object> headers)
        {
            try
            {
                var replyQueueName = $"{nameof(InventoryService)}ACK";

                var props = _channel?.CreateBasicProperties();
                if (props != null)
                {
                    props.ReplyTo = replyQueueName;

                    var body = Encoding.UTF8.GetBytes(message);
                    props.Headers = headers;
                    _channel.BasicPublish(exchangeName, routingKey, props, body);
                }

                return true;
            }
            catch
            {

                return false;
            }
        }

        #endregion Methods
    }
}
