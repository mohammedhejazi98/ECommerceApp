namespace InventoryService.EventProcessing
{
    public interface IOrderEventProcessor
    {
        #region Methods

        Task ProcessOrderEvent(string message);

        #endregion Methods

    }
}
