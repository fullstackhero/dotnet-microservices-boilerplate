using FSH.Microservices.Core.Exceptions;
using FSH.Microservices.Core.Serializers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FSH.Microservices.Infrastructure.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly ISerializerService _serializer;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, ISerializerService serializer, IWebHostEnvironment env)
    {
        _logger = logger;
        _serializer = serializer;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                throw new UnauthorizedException();
            }
        }
        catch (Exception exception)
        {
            var errorResult = exception switch
            {
                FluentValidation.ValidationException fluentException => ExceptionDetails.HandleFluentValidationException(fluentException),
                UnauthorizedException unauthorizedException => ExceptionDetails.HandleUnauthorizedException(unauthorizedException),
                NotFoundException notFoundException => ExceptionDetails.HandleNotFoundException(notFoundException),
                _ => ExceptionDetails.HandleDefaultException(exception),
            };

            LogErrorMessage(exception, errorResult);

            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.Status!.Value;
                await response.WriteAsync(_serializer.Serialize(errorResult));
            }
            else
            {
                _logger.LogWarning("Can't write error response. Response has already started.");
            }
        }
    }

    private void LogErrorMessage(Exception exception, ExceptionDetails details)
    {
        var properties = new Dictionary<string, object>
        {
            { "TraceId", details.TraceId }
        };

        if (details.Errors != null)
        {
            properties.Add("Errors", details.Errors);
        }

        if (_env.IsDevelopment())
        {
            properties.Add("StackTrace", exception.StackTrace!.Trim());
        }

        using (_logger.BeginScope(properties))
        {
            _logger.LogError("{title} | {details}", details.Title, details.Detail);
        }
    }
}
