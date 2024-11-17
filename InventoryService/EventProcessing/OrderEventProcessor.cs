using InventoryService.Common.Contracts.Infrastructure;
using InventoryService.Data;
using InventoryService.Entities;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace InventoryService.EventProcessing;

public class OrderEventProcessor(IServiceScopeFactory serviceScopeFactory) : IOrderEventProcessor
{
    #region Methods

    public async Task ProcessOrderEvent(string message)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        var consumer = scope.ServiceProvider.GetRequiredService<ISendToRabbitMqConsumer>();

        var result = DeserializeMessage<Order>(message);
        if (result is not null)
        {
            var product = await dbContext.Products.Where(x => x.Id == result.ProductId).FirstOrDefaultAsync();
            if (product is not null)
            {
                if (product.AvailableQuantity >= result.Quantity)
                {
                    product.AvailableQuantity -= result.Quantity;
                    dbContext.Products.Update(product);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("Inventory Updated");
                    await consumer.ProceedSendToConsumersDb("Inventory Updated", nameof(Order), result.Id.ToString(),
                        nameof(InventoryService), "OrderEventsNotify", "InventoryUpdated");
                }
                else
                {
                    Console.WriteLine($"Low Stock, available quantity ({product.AvailableQuantity}) in stock");
                    await consumer.ProceedSendToConsumersDb($"Low Stock, available quantity ({product.AvailableQuantity}) in stock", nameof(Order), result.Id.ToString(),
                        nameof(InventoryService), "OrderEventsNotify", "OutOfStock");

                }
            }
            else
            {
                Console.WriteLine("product not found");

            }
        }
    }

    private TMessage? DeserializeMessage<TMessage>(string message) where TMessage : class
    {
        return JsonConvert.DeserializeObject<TMessage>(message);
    }

    #endregion Methods

}