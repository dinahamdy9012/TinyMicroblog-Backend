using Microsoft.AspNetCore.Http;
using TinyMicroblog.Shared.Infrastructure.Security;

namespace TinyMicroblog.Shared.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;
        public AuthorizationMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the request contains an Authorization header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: No token provided");
                return;
            }

            try
            {
               _tokenService.ValidateToken(token);
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or expired token");
                return;
            }

            await _next(context); // Continue to next middleware
        }
    }
}
