using Microsoft.EntityFrameworkCore;
using TinyMicroblog.Domain.Entities;
using TinyMicroblog.Domain.Interfaces.Auth;
using TinyMicroblog.Shared.Infrastructure.Data;
using TinyMicroblog.Shared.Infrastructure.Repositories;

namespace TinyMicroblog.Auth.API.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TinyMicroblogDBContext _context;
        public UserRepository(TinyMicroblogDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking()
                       .FirstOrDefaultAsync(p => p.Username == username);
        }
    }
}
