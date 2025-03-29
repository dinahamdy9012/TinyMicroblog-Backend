using TinyMicroblog.Domain.Entities;

namespace TinyMicroblog.Domain.Interfaces.Auth
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
