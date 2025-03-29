using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TinyMicroblog.Domain.Data;
using TinyMicroblog.Domain.Entities;

namespace TinyMicroblog.Infrastructure.Auth.Repositories
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
            return await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                _context.Update(refreshToken);
                await _context.SaveChangesAsync();
            }
        }
    }
}
