namespace TinyMicroblog.Shared.Infrastructure.Security;

public interface ICookieService
{
    public void SetToken(string token);
    public void SetRefreshToken(string refreshToken);
    public string? GetToken();
    public string? GetRefreshToken();
    public void DeleteToken();
    void DeleteRefreshToken();
}
