namespace OrderPortal.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerPO { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Submitted { get; set; }
        public int LoginPK { get; set; }
    }
}
