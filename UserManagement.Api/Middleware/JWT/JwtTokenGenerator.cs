using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace UserManagement.Api.Middleware.JWT
{
    public static class JwtTokenGenerator
    {
        private static readonly string SecretKey = "RichKidAndTomerKey123!RichKidAndTomerKey123!";
        private static readonly string Issuer = "UserManagement.Api";
        private static readonly string Audience = "UserManagement.Api";

        public static string GenerateToken(string companyName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Company", companyName) 
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Issuer, 
                Audience = Audience, 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
