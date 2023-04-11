namespace WebAppProfile.Models
{
    public class EditView
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public IFormFile ImagePath { get; set; }
    }
}
