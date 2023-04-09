using System.ComponentModel.DataAnnotations;

namespace TTechEcommerceApi.Model
{
    public class AuthenticateRequestModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
