using TinyMicroblog.Application.Auth.ErrorCodes;
using TinyMicroblog.Application.Auth.Interfaces;
using TinyMicroblog.Auth.API.Application.Models;
using TinyMicroblog.Auth.API.Infrastructure.Repositories;
using TinyMicroblog.Domain.Interfaces.Auth;
using TinyMicroblog.Shared.Application.Models;
using TinyMicroblog.Shared.Infrastructure.Security;
using TinyMicroblog.SharedKernel.Exceptions;

namespace TinyMicroblog.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(ITokenService tokenService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<DataResponse<LoginResponseModel>> AuthenticateUser(LoginRequestModel request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrWhiteSpace(request.Username))
                throw new AppException(System.Net.HttpStatusCode.BadRequest, nameof(AuthErrorCodes.UsernameIsNullOrEmpty));

            if (string.IsNullOrEmpty(request.Password) || string.IsNullOrWhiteSpace(request.Password))
                throw new AppException(System.Net.HttpStatusCode.BadRequest, nameof(AuthErrorCodes.PasswordIsNullOrEmpty));

            DataResponse<LoginResponseModel> response = new DataResponse<LoginResponseModel>();

            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new AppException(System.Net.HttpStatusCode.NotFound, nameof(AuthErrorCodes.UserNotFound));

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
                throw new AppException(System.Net.HttpStatusCode.Unauthorized, nameof(AuthErrorCodes.IncorrectPassword));

            var (accessToken, refreshToken, refreshAccessToken) = _tokenService.GenerateTokens(user.Id, user.Username);
            await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);

            var loginResponse = new LoginResponseModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshAccessToken,
                ExpiresAt = refreshToken.ExpiryDate,
                UserId = user.Id,
            };
            return response.Success(loginResponse);
        }

        public async Task<DataResponse<LoginResponseModel>> RefreshAccessTokenAsync(string refreshAccessToken)
        {
            var refreshTokenHashed = _tokenService.HashToken(refreshAccessToken);
            var existingToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshTokenHashed);
            if (existingToken == null)
            {
                throw new AppException(System.Net.HttpStatusCode.Unauthorized, nameof(AuthErrorCodes.InvalidRefreshToken));
            }
            var user = await _userRepository.GetByIdAsync(existingToken.Id);
            var (newAccessToken, newRefreshToken, newAccessRefreshToken) = _tokenService.GenerateTokens(existingToken.UserId, user.Username);
            await _refreshTokenRepository.RevokeRefreshTokenAsync(existingToken.UserId);
            await _refreshTokenRepository.SaveRefreshTokenAsync(newRefreshToken);

            return new DataResponse<LoginResponseModel>().Success(new LoginResponseModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newAccessRefreshToken,
                ExpiresAt = newRefreshToken.ExpiryDate
            });
        }
    }
}
