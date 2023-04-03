namespace TTechEcommerceApi.Model
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string? NameFamily { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
