using OrderService.Data;
using OrderService.Entities;
using OrderService.Services.ServiceBus;
using System.Text.Json;

namespace OrderService.Common.Contracts.Infrastructure
{
    public interface ISendToRabbitMqConsumer
    {
        #region Methods

        Task ProceedSendToConsumersDb(string message, string entityName, string referenceId, string appId,
            string exchange, string routKey, CancellationToken cancellationToken = default);

        #endregion Methods
    }


    public class SendToRabbitMqConsumer(DataContext dbContext, IMessagesServiceBus messagesServiceBus)
        : ISendToRabbitMqConsumer
    {
        #region Methods

        public async Task ProceedSendToConsumersDb(string message, string entityName, string referenceId, string appId,
            string exchange, string routKey, CancellationToken cancellationToken = default)
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
                AppId = nameof(OrderService),
                EntityName = entityName,
                ReferenceId = referenceId
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            messagesServiceBus.PublishToConsumerDb(message, exchange, routKey, headers);
        }

        #endregion Methods
    }
}