using EcommerceApi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Shared.Enum;

namespace TTechEcommerceApi.Helper
{

    public class JwtUtilities : IJwtUtilities
    {
        private readonly JwtSettings jwtSettings;

        public JwtUtilities(IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

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
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim(JwtRegisteredClaimNames.Aud , jwtSettings.Audience!),
                    new Claim(JwtRegisteredClaimNames.Iss , jwtSettings.Issuer!)
                }),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
