namespace InventoryService.Entities
{
    /// <summary>
    /// Represents an order within the inventory system.
    /// </summary>
    public class Order
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the product associated with the order.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Represents the quantity of the product in the order.
        /// This property is used to track how many units of the product are included in a specific order.
        /// </summary>
        public decimal Quantity { get; set; }

        #endregion Public Properties
    }
}
