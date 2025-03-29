using System.ComponentModel.DataAnnotations;

namespace TinyMicroblog.Auth.API.Application.Models
{
    public class LoginRequestModel
    {
        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
