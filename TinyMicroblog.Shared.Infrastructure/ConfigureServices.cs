using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TinyMicroblog.Shared.Infrastructure.Data;
using TinyMicroblog.Shared.Infrastructure.Security;

namespace TinyMicroblog.Shared.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureSharedInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddDbContext<TinyMicroblogDBContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("TinyMicroblogDB")));
        return services;
    }
}
