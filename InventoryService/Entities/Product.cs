namespace InventoryService.Entities
{
    public class Product
    {
        #region Public Properties

        public decimal AvailableQuantity { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        #endregion Public Properties
    }
}
