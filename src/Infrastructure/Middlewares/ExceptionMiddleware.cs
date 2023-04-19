using FSH.Microservices.Core.Exceptions;
using FSH.Microservices.Core.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FSH.Microservices.Infrastructure.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly ISerializerService _serializer;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, ISerializerService serializer)
    {
        _logger = logger;
        _serializer = serializer;
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

            string errorId = Guid.NewGuid().ToString();


            var errorResult = new ErrorResult
            {
                Errors = new() { exception.Message.Trim() },
                ErrorId = errorId
            };

            var properties = new Dictionary<string, object>
            {
                { "ErrorId", errorResult.ErrorId},
                { "StackTrace", exception.StackTrace! },
                { "Source", exception.TargetSite?.DeclaringType?.FullName!},
                { "Errors", errorResult.Errors }
            };

            using (_logger.BeginScope(properties))
            {
                _logger.LogError(exception.Message);
            }

            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                await response.WriteAsync(_serializer.Serialize(errorResult));
            }
            else
            {
                _logger.LogWarning("Can't write error response. Response has already started.");
            }
        }
    }
}
