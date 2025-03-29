using TinyMicroblog.Servicebus.Interfaces;
using TinyMicroblog.Servicebus.Services;
using TinyMicroblog.Shared.UploadService.Interfaces;
using TinyMicroblog.Shared.UploadService.Services;
using Upload.API.Application.Interfaces;
using Upload.API.Application.Services;

namespace Upload.API;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureUploadServices(this IServiceCollection services)
    {
        services.AddScoped<IUploadService, AzureUploadService>();
        services.AddScoped<IUploadImageService, UploadImageService>();
        services.AddScoped<IServiceBusService, ServiceBusService>();
        return services;
    }
}
