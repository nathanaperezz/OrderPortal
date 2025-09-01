namespace OrderPortal.Models
{
    public class CustomerShipTo
    {
        public int CustomerShipToPK { get; set; }
        public int CustomerId { get; set; }
        public string? ShipToAddressId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
    }
}
