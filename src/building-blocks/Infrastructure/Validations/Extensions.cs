using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Infrastructure.Validations;

public static class Extensions
{
    public static IServiceCollection RegisterValidators(this IServiceCollection services, Assembly assemblyContainingValidators)
    {
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(assemblyContainingValidators);
        return services;
    }
}
