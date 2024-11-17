using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Threading.Channels;
//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
var factory = new ConnectionFactory
{
    HostName = "rabbitmq", // Use the actual IP address or a correct hostname
    UserName = "guest", // Use your actual username
    Password = "guest"  // Use your actual password
};
//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
using var channel = connection.CreateModel();
//declare the queue after mentioning name and a few property related to that
var exchange = "OrderEventsNotify";
string queueName = "OrderEventsQty";

channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true);

channel.QueueDeclare(queueName, autoDelete: false, exclusive: false, durable: true);
channel.QueueBind(queueName, exchange, "");

//Set Event object which listen message from chanel which is sent by producer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    if (eventArgs.RoutingKey == "OutOfStock")
        Console.WriteLine($"Product message received: {message}");
    
    if (eventArgs.RoutingKey == "InventoryUpdated")
        Console.WriteLine($"Product message received: {message}");
};
//read the message
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
while (true)
{
    
}