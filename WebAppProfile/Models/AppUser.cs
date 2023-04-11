using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAppProfile.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Path { get; set; }
        [NotMapped]
        [Display(Name = "Choose Image")]
        public IFormFile ImagePath { get; set; }
    }
}
