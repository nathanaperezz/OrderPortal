namespace OrderPortal.Models
{
    public class Login
    {
        public int LoginPK { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public int CustomerId { get; set; }
    }
}
