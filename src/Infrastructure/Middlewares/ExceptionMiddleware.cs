using FSH.Microservices.Core.Exceptions;
using FSH.Microservices.Core.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;

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
            var properties = new Dictionary<string, object>
            {
                { "ErrorId", errorId},
                { "StackTrace", exception.StackTrace! }
            };

            using (_logger.BeginScope(properties))
            {
                _logger.LogError(exception.Message);
            }

            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId
            };

            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                await response.WriteAsync(_serializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}
