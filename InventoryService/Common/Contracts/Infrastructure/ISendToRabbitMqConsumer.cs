using System.Text.Json;
using InventoryService.Data;
using InventoryService.Entities;
using InventoryService.Services.ServiceBus;

namespace InventoryService.Common.Contracts.Infrastructure
{
    public interface ISendToRabbitMqConsumer
    {
        #region Methods

        Task ProceedSendToConsumersDb(string message, string entityName, string referenceId, string appId, string exchange, string routKey, CancellationToken cancellationToken = default);
        
        #endregion Methods
    }


    public class SendToRabbitMqConsumer(DataContext dbContext, IMessagesServiceBus messagesServiceBus) : ISendToRabbitMqConsumer
    {
        #region Methods
        public async Task ProceedSendToConsumersDb(string message, string entityName, string referenceId, string appId, string exchange, string routKey, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> headers = new();

            var outboxId = Guid.NewGuid();
            await dbContext.Outbox.AddAsync(new Outbox
            {
                Id = outboxId,
                CreatedOn = DateTime.Now,
                Exchange = exchange,
                RoutKey = routKey,
                Message = message,
                Headers = JsonSerializer.Serialize(headers),
                Completed = false,
                AppId = nameof(InventoryService),
                EntityName = entityName,
                ReferenceId = referenceId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

                messagesServiceBus.PublishToConsumerDb(message, exchange, routKey, headers);

        }



        #endregion Methods


    }
}
