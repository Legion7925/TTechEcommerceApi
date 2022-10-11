using EcommerceApi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TTechEcommerceApi.Interface;

namespace TTechEcommerceApi.Helper
{

    public class JwtUtilities : IJwtUtilities
    {
        private readonly AppSettings appSettings;

        public JwtUtilities(IOptions<AppSettings> appSettings)
        {
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
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim(JwtRegisteredClaimNames.Aud , appSettings.Audience!),
                    new Claim(JwtRegisteredClaimNames.Iss , appSettings.Issuer!)
                }),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
