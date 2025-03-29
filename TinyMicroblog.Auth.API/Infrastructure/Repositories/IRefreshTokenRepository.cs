using TinyMicroblog.Domain.Entities;

namespace TinyMicroblog.Auth.API.Infrastructure.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(int userId);
    }
}
