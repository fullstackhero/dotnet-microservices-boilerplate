using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FSH.Microservices.Infrastructure.Behaviors;
public static class Extensions
{
    public static IServiceCollection AddBehaviors(this IServiceCollection services, Assembly assemblyContainingValidators)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
