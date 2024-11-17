using InventoryService.Common.Constants.RabbitMq;
using InventoryService.Entities;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using InventoryService.EventProcessing;

// ReSharper disable LocalizableElement

namespace InventoryService.BackgroundServices
{
    /// <summary>
    /// Represents a background service for subscribing to tenant-specific messages.
    /// </summary>
    public class OrderMessageSubscriber : BackgroundService
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly IOrderEventProcessor _eventProcessor;


        private IModel? _channel;

        /// /
        private IConnection? _connection;

        private string _queueName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Represents a background service that subscribes to tenant messages.
        /// </summary>
        public OrderMessageSubscriber(IConfiguration configuration, IOrderEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitializeRabbitMqConfiguration();
        }

        #endregion Constructors

        #region Methods

        // Dispose Method
        /// <summary>
        /// Method to release resources used by the object.
        /// </summary>
        /// <remarks>
        /// This method is called when the object is no longer needed and its resources should be released.
        /// It closes the RabbitMQ channel and connection if they are open and then calls the base class's Dispose method.
        /// </remarks>
        public override void Dispose()
        {
            if (_channel is { IsOpen: true })
            {
                _channel.Close();
                _connection?.Close();
            }
            base.Dispose();
        }

        /// <summary>
        /// Executes the asynchronous operation of the tenant message subscriber.
        /// </summary>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            ulong deliveryTag = 0;

            consumer.Received += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    deliveryTag = ea.DeliveryTag;

                    if (ea.RoutingKey == "OrderCreated")
                        await _eventProcessor.ProcessOrderEvent(message);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception caught in consumer.Received: {e.Message}");
                    _channel?.BasicReject(deliveryTag, false);
                }
            };
            _channel.BasicConsume(_queueName, true, consumer);

            // Ensure autoAck is set to false

            return Task.CompletedTask;
        }

        /// /
        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection shutdown");
        }

        private void InitializeRabbitMqConfiguration()
        {

            try
            {
                var exchange = "OrderEvents";
                string queueName = "OrderEvents";

                _connection = RabbitMqConnection.GetConnection();
                _channel = _connection?.CreateModel();
                _channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true);

                _queueName = queueName;
                _channel.QueueDeclare(queueName, autoDelete: false, exclusive: false, durable: true);
                _channel.QueueBind(_queueName, exchange, "");
                if (_connection != null) _connection.ConnectionShutdown += _connection_ConnectionShutdown;
                Console.WriteLine($"Connection {exchange} has been created");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to the rabbitmq: {ex.Message}");
            }
        }

        #endregion Methods

        private TMessage? DeserializeMessage<TMessage>(string message) where TMessage : class
        {
            return JsonConvert.DeserializeObject<TMessage>(message);
        }
    }
}
