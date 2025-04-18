using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagement.Api.Middleware
{
    public static class JwtValidator
    {

        private static readonly List<string> AllowCompanies = new List<string>
        {
            "RICHKID",
            "Google",
            "Microsoft",
            "Apple",
            "Amazon",
            "Facebook",

        };

        private static readonly string SecretKey = "RichKidAndTomerKey123!RichKidAndTomerKey123!";
        private static readonly string Issuer = "UserManagement.Api";
        private static readonly string Audience = "UserManagement.Api";

      
        public static bool IsTokenValid(string token, out string? companyName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "UserManagement.Api", 
                ValidAudience = "UserManagement.Api", 
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), 
            };

            try
            {
             
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                companyName = principal.FindFirst("Company")?.Value;

          
                return !string.IsNullOrEmpty(companyName) && AllowCompanies.Contains(companyName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JWT validation failed: {ex.Message}");
                companyName = null;
                return false;
            }
        }
    }
}
