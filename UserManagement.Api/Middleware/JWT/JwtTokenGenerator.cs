using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Api.Models.DTOS;

namespace UserManagement.Api.Middleware.JWT
{
    public static class JwtTokenGenerator
    {
        private static readonly IConfiguration Configuration;
        private static readonly string SecretKey;
        private static readonly string Issuer;
        private static readonly string Audience;

        static JwtTokenGenerator()
        {
            // in a real application we should save those values in a config file or environment variables
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            SecretKey = Configuration["JwtSettings:SecretKey"]!;
            Issuer = Configuration["JwtSettings:ValidIssuer"]!;
            Audience = Configuration["JwtSettings:ValidAudience"]!;
        }

        /*
         * After a successful login, this function will be called to generate a JWT token
         * and in the future, this token will be used to authenticate the user
         */
        public static string GenerateToken(Credentials ManagerCred)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Company", ManagerCred.Company),
                    new Claim("UserName", ManagerCred.UserName),
                    new Claim("Password", ManagerCred.Password),

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
