namespace TTechEcommerceApi.Model
{
    public class AuthenticateResponseModel
    {
        public int Id { get; set; }

        public string? NameFamily { get; set; }

        public string? Username { get; set; }

        public string? JwtToken { get; set; }
    }
}
