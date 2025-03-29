namespace TinyMicroblog.Application.Auth.ErrorCodes
{
    public enum AuthErrorCodes
    {
        UsernameIsNullOrEmpty,
        PasswordIsNullOrEmpty,
        UserNotFound,
        IncorrectPassword,
        InvalidRefreshToken
    }
}
