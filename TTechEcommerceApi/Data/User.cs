using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcommerceApi.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string? NameFamily { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [MaxLength(10)]
        public string? PostalCode { get; set; }

        public string? Address { get; set; }

        public Role Role { get; set; }

        [Required]
        [JsonIgnore]
        [MaxLength(150)]
        public string? PasswordHash { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public string? Email { get; set; }

        public string? ResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public DateTime? PasswordReset { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
