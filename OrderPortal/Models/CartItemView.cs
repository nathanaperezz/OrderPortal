namespace OrderPortal.Models
{
    public class CartItemView
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? Pallets { get; set; }
        public decimal ExtendedAmount { get; set; }
    }
}
