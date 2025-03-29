namespace TinyMicroblog.Auth.API.Application.Models
{
    public class LoginResponseModel
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
