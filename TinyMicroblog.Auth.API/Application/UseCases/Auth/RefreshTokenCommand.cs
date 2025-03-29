using MediatR;
using TinyMicroblog.Auth.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Auth.API.Application.UseCases.Auth
{
    public class RefreshTokenCommand : IRequest<DataResponse<LoginResponseModel>>
    {
        public RefreshTokenRequestModel RefreshTokenRequestModel { get; }
        public RefreshTokenCommand(RefreshTokenRequestModel refreshTokenRequestModel)
        {
            RefreshTokenRequestModel = refreshTokenRequestModel;
        }
    }
}
