using MediatR;
using TinyMicroblog.Auth.API.Application.UseCases.Auth;
using TinyMicroblog.Shared.Application.Models;
using TinyMicroblog.Application.Auth.Interfaces;
using TinyMicroblog.Auth.API.Application.Models;

namespace EndUserManagement.Application.UseCases.EndUser
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, DataResponse<LoginResponseModel>>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }
        public async Task<DataResponse<LoginResponseModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshAccessTokenAsync(request.RefreshTokenRequestModel.RefreshToken);
        }
    }
}
