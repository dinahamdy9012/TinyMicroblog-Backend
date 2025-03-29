using System.IdentityModel.Tokens.Jwt;
using TinyMicroblog.Domain.Entities;

namespace TinyMicroblog.Shared.Infrastructure.Security
{
    public interface ITokenService
    {
        (string AccessToken, RefreshToken RefreshToken, string RefreshAccessToken) GenerateTokens(int userId, string username);

        string HashToken(string token);
        JwtSecurityToken? ValidateToken(string token);
    }
}
