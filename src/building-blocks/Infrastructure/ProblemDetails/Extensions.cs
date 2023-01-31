using FSH.Core.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace FSH.Infrastructure.Common;

public static class Extensions
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        return services.AddProblemDetails(config =>
        {
            config.IncludeExceptionDetails = (_, _) => false;

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            config.MapToStatusCode<Ardalis.GuardClauses.NotFoundException>(StatusCodes.Status404NotFound);
        });
    }
}
