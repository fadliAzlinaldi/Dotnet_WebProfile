namespace WebAppProfile.Models
{
    public class RegisterView
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; }
        public string City { get; set; }
        public IFormFile ImagePath { get; set; }
    }
}
