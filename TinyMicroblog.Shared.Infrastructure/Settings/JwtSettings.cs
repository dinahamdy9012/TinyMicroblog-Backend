namespace TinyMicroblog.Shared.Infrastructure.Settings;

public class JwtSettings
{
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public int AccessTokenExpirationMinutes { get; set; }
    public string Audience { get; set; } = null!;
}