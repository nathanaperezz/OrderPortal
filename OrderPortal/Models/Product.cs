namespace OrderPortal.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public string? UoM { get; set; }
        public int? EachesToPallet { get; set; }
        public string? Image { get; set; }
    }
}
