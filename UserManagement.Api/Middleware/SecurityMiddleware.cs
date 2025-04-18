using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Api.Middleware.JWT;

namespace UserManagement.Api.Middleware
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        // in this method we check if the request is authorized
        // every api request will pass through this method unless it is marked with [AllowAnonymous]
        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            if (!IpValidator.IsIpAllowed(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied: Your IP is not allowed.");
                return;
            }


            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied: Missing or invalid JWT.");
                return;
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            if (!JwtValidator.IsTokenValid(token,out var CompanyName))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync($"Access denied: Invalid JWT token");
                return;
            }

            await _next(context);
        }
    }
}
