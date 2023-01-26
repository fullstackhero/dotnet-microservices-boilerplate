using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Core.Mediator;

public static class Extensions
{
    public static IServiceCollection RegisterMediatR(this IServiceCollection services, Assembly mediatrAssembly)
    {
        services.AddMediatR(mediatrAssembly);
        return services;
    }
}
