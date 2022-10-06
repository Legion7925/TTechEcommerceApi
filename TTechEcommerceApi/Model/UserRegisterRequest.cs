using System.ComponentModel.DataAnnotations;

namespace TTechEcommerceApi.Model
{
    public class UserRegisterRequest
    {
        [Required]
        public string? NameFamily { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [MaxLength(10)]
        public string? PostalCode { get; set; }

        public string? Address { get; set; }

    }
}
