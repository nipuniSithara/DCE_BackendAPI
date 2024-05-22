namespace Data_Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public decimal UnitPrice { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        //public virtual Supplier? Supplier { get; set; }
    }
}