

// ReSharper disable LocalizableElement

using OrderService.Common.Constants.RabbitMq;

using RabbitMQ.Client;

using System.Text;

namespace OrderService.Services.ServiceBus
{
    /// <summary>
    /// Service that provides methods for interacting with a service bus to publish messages.
    /// </summary>
    public class MessagesServiceBus() : IMessagesServiceBus
    {
        #region Fields

        /// <summary>
        /// Represents the channel used for communication with RabbitMQ in the MessagesServiceBus class.
        /// The channel is responsible for sending messages, declaring exchanges, and managing connection lifecycle events.
        /// </summary>
        private IModel _channel;

        /// <summary>
        /// Represents the connection interface to the RabbitMQ server.
        /// This connection is used to create channels for message publishing and subscribing.
        /// It is established in the <c>Connect</c> method and should be checked for openness before publishing messages.
        /// </summary>
        private IConnection _connection;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Publishes a message to the specified exchange in the RabbitMQ broker using the provided routing key and headers.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="exchangeName">The name of the exchange to which the message is published.</param>
        /// <param name="routingKey">The routing key used by the exchange to determine the message's destination queue.</param>
        /// <param name="headers">A dictionary of additional headers to include in the message.</param>
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


        /// <summary>
        /// Handles the event when the RabbitMQ connection is shut down.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data containing details about the shutdown event.</param>
        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //_connection.Dispose();
            Console.WriteLine("Connection has been shutdown");
        }

        /// Establishes a connection to the RabbitMQ server and sets up an exchange.
        /// <param name="exchangeName">The name of the exchange to declare and use in the connection.</param>
        private void Connect(string exchangeName)
        {   
            
            var factory = new ConnectionFactory
            { 
                HostName = "rabbitmq", 
                UserName = "guest", 
                Password = "guest"  
            };
            _connection =  factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;
            Console.WriteLine("Connection has been created");
        }

        /// <summary>
        /// Sends a message to the specified RabbitMQ exchange with the given routing key and headers.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="exchangeName">The name of the exchange to which the message should be published.</param.
        /// <param name="routingKey">The routing key to use for message delivery.</param>
        /// <param name="headers">A dictionary containing any headers to include with the message.</param>
        /// <returns>True if the message was sent successfully, otherwise false.</returns>
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
