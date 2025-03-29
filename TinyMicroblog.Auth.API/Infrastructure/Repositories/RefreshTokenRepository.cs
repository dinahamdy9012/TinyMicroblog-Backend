using Microsoft.EntityFrameworkCore;
using TinyMicroblog.Domain.Entities;
using TinyMicroblog.Shared.Infrastructure.Data;

namespace TinyMicroblog.Auth.API.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TinyMicroblogDBContext _context;
        public RefreshTokenRepository(TinyMicroblogDBContext context)
        {
            _context = context;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Token == token && x.ExpiryDate > DateTime.UtcNow);
        }

        public async Task RevokeRefreshTokenAsync(int userId)
        {
            var tokens = _context.RefreshTokens.Where(t => t.UserId == userId);
            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }
    }
}
