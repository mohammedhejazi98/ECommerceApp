namespace OrderService.Entities
{
    public class Outbox 
    {
        #region Properties

        public DateTime CreatedOn { get; set; }
        public string Exchange { get; set; }
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string RoutKey { get; set; }
        public string Headers { get; set; }
        public bool Completed { get; set; }
        public string AppId { get; set; }
        public string ReferenceId { get; set; }
        public string EntityName { get; set; }
        #endregion Properties
    }
}
