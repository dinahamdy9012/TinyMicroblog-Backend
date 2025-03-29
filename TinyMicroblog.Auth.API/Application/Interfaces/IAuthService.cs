using TinyMicroblog.Auth.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<DataResponse<LoginResponseModel>> AuthenticateUser(LoginRequestModel request);
        Task<DataResponse<LoginResponseModel>> RefreshAccessTokenAsync(string refreshAccessToken);
    }
}
