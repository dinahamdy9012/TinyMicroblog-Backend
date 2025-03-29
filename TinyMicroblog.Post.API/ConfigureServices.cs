using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Post.API.Application.Interfaces;
using TinyMicroblog.Post.API.Application.Services;
using TinyMicroblog.Post.API.Infrastructure.Repositories;
using TinyMicroblog.Servicebus.Interfaces;
using TinyMicroblog.Servicebus.Services;
using TinyMicroblog.Shared.Application.Interfaces;
using TinyMicroblog.Shared.Application.Services;
using TinyMicroblog.Shared.Infrastructure.Repositories;

namespace TinyMicroblog.Post.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPostImageService, PostImageService>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostImageRepository, PostImageRepository>();
        services.AddAutoMapper(System.Reflection.Assembly.GetEntryAssembly(), typeof(Program).Assembly);
        services.AddScoped<IServiceBusService, ServiceBusService>();
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}
