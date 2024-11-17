namespace OrderService.Dto
{
    public class OrderItem
    {
        #region Public Properties

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }

        #endregion Public Properties
    }
}
