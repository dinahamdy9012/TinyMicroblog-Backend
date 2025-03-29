using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TinyMicroblog.Domain.Entities;
using TinyMicroblog.Shared.Infrastructure.Settings;

namespace TinyMicroblog.Shared.Infrastructure.Security
{
    public class TokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public (string AccessToken, RefreshToken RefreshToken, string RefreshAccessToken) GenerateTokens(int userId, string username)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtSettings.AccessTokenExpirationMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var refreshAccessToken = GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = HashToken(refreshAccessToken),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(7), // Refresh token valid for 7 days
            };
            return (tokenHandler.WriteToken(accessToken), refreshToken, refreshAccessToken);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }

        public JwtSecurityToken? ValidateToken(string token)
        {
            // Validate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Set true if you have a specific issuer
                ValidateAudience = false, // Set true if you have a specific audience
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception ex) 
            { 
                throw new Exception(token, ex);
            }
        }
    }
}
