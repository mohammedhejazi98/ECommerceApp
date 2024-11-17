namespace InventoryService.Entities
{
    public class Order
    {
        #region Public Properties

        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }

        #endregion Public Properties
    }
}
