namespace ParkyWeb.Models
{
    public class UserWeb
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; } = "Visitor";
        public string Token { get; set; }
    }
}
