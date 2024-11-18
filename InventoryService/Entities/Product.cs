namespace InventoryService.Entities
{
    /// <summary>
    /// Represents a product in the inventory service.
    /// </summary>
    public class Product
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the available quantity of the product.
        /// This property indicates the inventory level for the product.
        /// </summary>
        public decimal AvailableQuantity { get; set; }

        /// Gets or sets the unique identifier for the product.
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        /// <value>
        /// The monetary amount required to purchase the product.
        /// </value>
        public decimal Price { get; set; }
        
        #endregion Public Properties
    }
}
