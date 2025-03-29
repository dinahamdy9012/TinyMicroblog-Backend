using MediatR;
using TinyMicroblog.Auth.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Auth.API.Application.UseCases.Auth
{
    public class UserLoginCommand : IRequest<DataResponse<LoginResponseModel>>
    {
        public LoginRequestModel LoginRequestModel { get; }
        public UserLoginCommand(LoginRequestModel loginRequestModel)
        {
            LoginRequestModel = loginRequestModel;
        }
    }
}
