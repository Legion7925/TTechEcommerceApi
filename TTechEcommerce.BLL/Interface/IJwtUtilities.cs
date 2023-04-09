using EcommerceApi.Entities;

namespace TTechEcommerceApi.Interface
{
    public interface IJwtUtilities
    {
        string GenerateJwtToken(User user);
    }
}
