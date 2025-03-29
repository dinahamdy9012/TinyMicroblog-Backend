namespace TinyMicroblog.Shared.Infrastructure.Security;

public interface ICurrentUserService
{
    (int userId, string username) GetCurrentUser();
    string GetCurrentUserCountry();
}
