using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])(?=.{6,}).*$",
         ErrorMessage = "Password must have at least 1 uppercase letter, 1 lowercase letter, 1 number, 1 special character, and be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
