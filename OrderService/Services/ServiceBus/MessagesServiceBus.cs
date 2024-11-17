

// ReSharper disable LocalizableElement

using OrderService.Common.Constants.RabbitMq;

using RabbitMQ.Client;

using System.Text;

namespace OrderService.Services.ServiceBus
{
    public class MessagesServiceBus() : IMessagesServiceBus
    {
        #region Fields

        private IModel _channel;
        private IConnection _connection;

        #endregion Fields

        #region Methods

        public void PublishToConsumerDb(string message, string exchangeName, string routingKey, Dictionary<string, object> headers)
        {
            //if (_connection is null)
            Connect(exchangeName);

            if (_connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ Connection Is Open");
                var result = SendMessage(message, exchangeName, routingKey, headers);
                _channel.Close();
                if (result)
                    Console.WriteLine($"{message} has been sent");
            }
        }


        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //_connection.Dispose();
            Console.WriteLine("Connection has been shutdown");
        }
        private void Connect(string exchangeName)
        {   
            
            var factory = new ConnectionFactory
            { 
                HostName = "rabbitmq", // Use the actual IP address or a correct hostname
                UserName = "guest", // Use your actual username
                Password = "guest"  // Use your actual password
            };
            _connection =  factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;
            Console.WriteLine("Connection has been created");
        }

        private bool SendMessage(string message, string exchangeName, string routingKey, Dictionary<string, object> headers)
        {
            try
            {
                var replyQueueName = $"{nameof(OrderService)}ACK";

                var props = _channel.CreateBasicProperties();
                props.ReplyTo = replyQueueName;

                var body = Encoding.UTF8.GetBytes(message);
                props.Headers = headers;
                _channel.BasicPublish(exchangeName, routingKey, props, body);

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
