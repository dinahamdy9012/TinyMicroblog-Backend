using TinyMicroblog.Domain.Entities;

namespace TinyMicroblog.Infrastructure.Auth.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);
    }
}
