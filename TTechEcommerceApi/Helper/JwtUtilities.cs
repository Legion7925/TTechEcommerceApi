using EcommerceApi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TTechEcommerceApi.Helper
{
    public class JwtUtilities
    {
        private readonly EcommerceContext context;
        private readonly AppSettings appSettings;

        public JwtUtilities(EcommerceContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            var userRole = "";

            switch (user.Role)
            {
                case Role.Admin:
                    userRole = "Admin";
                    break;
                case Role.User:
                    userRole = "User";
                    break;
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateJwtToken(string? token)
        {
            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();

            return false;
        }
    }
}
