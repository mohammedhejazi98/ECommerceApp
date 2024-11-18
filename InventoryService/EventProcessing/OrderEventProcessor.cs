using InventoryService.Common.Contracts.Infrastructure; 
using InventoryService.Data; 
using InventoryService.Entities; 
using Microsoft.EntityFrameworkCore; 
using Newtonsoft.Json; 

namespace InventoryService.EventProcessing;

/// <summary>
/// The OrderEventProcessor class is responsible for processing order events received as messages.
/// </summary>
/// <remarks>
/// It handles updating the product inventory based on the order details, and notifying consumers
/// about the order processing result using RabbitMQ.
/// </remarks>
/// <param name="serviceScopeFactory">Factory to create service scopes for dependency injection.</param>
public class OrderEventProcessor(IServiceScopeFactory serviceScopeFactory) : IOrderEventProcessor // class implementation
{
    #region Methods

    /// <summary>
    /// Processes the incoming order event message.
    /// </summary>
    /// <param name="message">The message containing order details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessOrderEvent(string message) // asynchronous method to process order event
    {
        using var scope = serviceScopeFactory.CreateScope(); // create a scope using service scope factory
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>(); // get DataContext service
        var consumer = scope.ServiceProvider.GetRequiredService<ISendToRabbitMqConsumer>(); // get RabbitMQ consumer service

        var result = DeserializeMessage<Order>(message); // deserialize message to Order object
        if (result is not null) // check if result is not null
        {
            var product = await dbContext.Products.Where(x => x.Id == result.ProductId).FirstOrDefaultAsync(); // find product by Id
            if (product is not null) // check if product is not null
            {
                if (product.AvailableQuantity >= result.Quantity) // check if available quantity is sufficient
                {
                    product.AvailableQuantity -= result.Quantity; // reduce available quantity
                    dbContext.Products.Update(product); // update product in database
                    await dbContext.SaveChangesAsync(); // save changes to database
                    Console.WriteLine("Inventory Updated"); // log inventory update message
                    await consumer.ProceedSendToConsumersDb("Inventory Updated", nameof(Order), result.Id.ToString(),
                        nameof(InventoryService), "OrderEventsNotify",
                        "InventoryUpdated"); // send success message to consumers
                }
                else
                {
                    await consumer.ProceedSendToConsumersDb(
                        $"Low Stock Alert: Current available quantity is ({product.AvailableQuantity}).", nameof(Order),
                        result.Id.ToString(), nameof(InventoryService), "OrderEventsNotify",
                        "OutOfStock"); // send low stock alert to consumers
                }
            }
            else
            {
                Console.WriteLine("product not found"); // log product not found message
            }
        }
    }

    /// <summary>
    /// Deserializes a JSON string into an object of type TMessage.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object to deserialize to.</typeparam>
    /// <param name="message">The JSON string to deserialize.</param>
    /// <returns>The deserialized object of type TMessage, or null if deserialization fails.</returns>
    private TMessage? DeserializeMessage<TMessage>(string message) where TMessage : class // generic method to deserialize message
    {
        return JsonConvert.DeserializeObject<TMessage>(message); // return deserialized message
    }

    #endregion Methods // end region for methods
}