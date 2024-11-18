using System.Text; // Import required namespace for text encoding/decoding.
using Microsoft.Extensions.DependencyInjection; // Import DI services namespace.
using Microsoft.Extensions.Logging; // Import logging services namespace.
using RabbitMQ.Client; // Import RabbitMQ client library.
using RabbitMQ.Client.Events; // Import RabbitMQ client events library.

namespace NotificationService; // Define namespace for the NotificationService.

internal class Program // Define a class named Program.
{
    public static async Task Main(string[] args) // Define the main entry point method.
    {
        var serviceCollection = new ServiceCollection(); // Create a new service collection for DI.
        ConfigureServices(serviceCollection); // Configure services for DI in the collection.

        var serviceProvider = serviceCollection.BuildServiceProvider(); // Build service provider for DI.
        var logger = serviceProvider.GetService<ILogger<Program>>(); // Get the logger service.

        logger!.LogInformation("Application starting..."); // Log an information message.

        var factory = new ConnectionFactory // Create a new connection factory.
        {
            HostName = "rabbitmq", // Set RabbitMQ host name.
            UserName = "guest", // Set RabbitMQ username.
            Password = "guest" // Set RabbitMQ password.
        };

        var connection = factory.CreateConnection(); // Create a new connection to RabbitMQ.
        using var channel = connection.CreateModel(); // Create a new channel from the connection.

        var exchange = "OrderEventsNotify"; // Define exchange name.
        string queueName = "OrderEventsQty"; // Define queue name.

        channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true); // Declare an exchange on the channel.
        channel.QueueDeclare(queueName, autoDelete: false, exclusive: false, durable: true); // Declare a queue on the channel.
        channel.QueueBind(queueName, exchange, ""); // Bind the queue to the exchange.

        var consumer = new EventingBasicConsumer(channel); // Create a new event-driven consumer.
        consumer.Received += (_, eventArgs) => // Subscribe to the Received event.
        {
            var body = eventArgs.Body.ToArray(); // Get the message body as byte array.
            var message = Encoding.UTF8.GetString(body); // Convert the byte array to string.

            if (eventArgs.RoutingKey == "OutOfStock") // Check if the routing key is "OutOfStock".
                logger!.LogInformation($"Product message received: {message}"); // Log the received message.

            if (eventArgs.RoutingKey == "InventoryUpdated") // Check if the routing key is "InventoryUpdated".
                logger!.LogInformation($"Product message received: {message}"); // Log the received message.
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer); // Start message consumption from the queue.

        await Task.Delay(Timeout.Infinite); // Keep the application running indefinitely.
    }

    private static void ConfigureServices(IServiceCollection services) // Define method to configure DI services.
    {
        services.AddLogging(configure => // Add logging services to the service collection.
        {
            configure.AddConsole(); // Add console logging.
            configure.SetMinimumLevel(LogLevel.Information); // Set minimum log level to Information.
        });
    }
}