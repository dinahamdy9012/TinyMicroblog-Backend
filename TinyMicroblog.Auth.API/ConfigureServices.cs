using TinyMicroblog.Application.Auth.Interfaces;
using TinyMicroblog.Application.Auth.Services;
using TinyMicroblog.Auth.API.Infrastructure.Repositories;
using TinyMicroblog.Domain.Interfaces.Auth;

namespace TinyMicroblog.Auth.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
