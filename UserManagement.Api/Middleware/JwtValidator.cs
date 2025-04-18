using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using UserManagement.Api.Models.DTOS;
using UserManagement.Api.Repositories;


namespace UserManagement.Api.Middleware
{
    public static class JwtValidator
    {
        private static readonly IConfiguration Configuration;
        private static readonly string SecretKey;
        private static readonly string ValidIssuer;
        private static readonly string ValidAudience;

        static JwtValidator()
        {
            /*
              In a real-world production system, sensitive values like the SecretKey
              should be stored in environment variables
             */
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            SecretKey = Configuration["JwtSettings:SecretKey"]!;
            ValidIssuer = Configuration["JwtSettings:ValidIssuer"]!;
            ValidAudience = Configuration["JwtSettings:ValidAudience"]!;
        }

        /*
         * in this function we will validate the token by checking if the credentials are valid (username, password, company)
           only users that are in the list of users can access the system.
         */
        public static bool IsTokenValid(string token, out Credentials? credManager)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ValidIssuer,
                ValidAudience = ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var credentials = new Credentials
                {
                    UserName = principal.FindFirst("UserName")?.Value ?? string.Empty,
                    Password = principal.FindFirst("Password")?.Value ?? string.Empty,
                    Company = principal.FindFirst("Company")?.Value ?? string.Empty
                };

                credManager = credentials;
                return UserRepository.AuthenticateUser(credentials.UserName, credentials.Password, credentials.Company);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JWT validation failed: {ex.Message}");
              
            }
            credManager = null;
            return false;
        }

    }
}
