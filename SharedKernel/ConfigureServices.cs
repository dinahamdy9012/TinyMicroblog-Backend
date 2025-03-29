using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace TinyMicroblog.SharedKernel;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureSharedServices(this IServiceCollection services)
    {
        return services;
    }
}
