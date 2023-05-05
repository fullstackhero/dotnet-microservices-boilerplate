using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Framework.Infrastructure.Options;

public static class Extensions
{
    public static T LoadOptions<T>(this IConfiguration configuration, string sectionName) where T : IOptionsRoot
    {
        var options = configuration.GetSection(sectionName).Get<T>();
        // handle case where options is null
        // create a new instance and returns the default class
        if (options == null) return (T)Activator.CreateInstance(typeof(T))!;
        return options;
    }

    public static T BindValidateReturn<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IOptionsRoot
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return configuration.LoadOptions<T>(typeof(T).Name);
    }
    public static void BindValidate<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IOptionsRoot
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

}
