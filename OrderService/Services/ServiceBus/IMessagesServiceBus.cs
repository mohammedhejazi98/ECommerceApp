namespace OrderService.Services.ServiceBus
{
    public interface IMessagesServiceBus
    {
        #region Methods
        // Lookups , Countries , Cities, Regions,Districts,Payment Method Type , Currency , Country Currency , Courier Company
        void PublishToConsumerDb(string message, string exchangeName, string routingKey, Dictionary<string, object> headers);

        #endregion Methods
    }
}
