using Infrastructure.UploadService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace UploadService;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureUploadServices(this IServiceCollection services)
    {
        services.AddScoped<IUploadService, Infrastructure.UploadService.Services.UploadService>();

        return services;
    }
}
