using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;
using TinyMicroblog.Shared.Infrastructure.Settings;

namespace TinyMicroblog.Shared.Infrastructure.Security;

public class CookieService(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings) : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public void SetToken(string token) => _httpContextAccessor.HttpContext?.Response.Cookies.Append("token_key", token, new CookieOptions
    {
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Secure = true,
        MaxAge = TimeSpan.FromMinutes(_jwtSettings.AccessTokenExpirationMinutes)
    });

    public void SetRefreshToken(string refreshToken) => _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh_token_key", refreshToken, new CookieOptions
    {
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Secure = true,
    });

    public void DeleteToken() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token_key");

    public void DeleteRefreshToken() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refresh_token_key");

    public string? GetToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["token_key"];
        if(string.IsNullOrEmpty(token))
        {
           if(_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues headerValue))
            {
                token = headerValue.ToString().Replace("Bearer ", string.Empty);
            }

        }
        return token;
    }

    public string? GetRefreshToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["refresh_token_key"];
        return token;
    }
}
