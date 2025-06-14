namespace CozyHaven.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public string Composition { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public decimal SupplierPrice { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
