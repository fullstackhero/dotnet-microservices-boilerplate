using FSH.Microservices.Core.Exceptions;
using FSH.Microservices.Core.Serializers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
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

            string errorId = Guid.NewGuid().ToString();


            var errorResult = new ErrorResult
            {
                Title = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode),
                Instance = errorId,
                Detail = exception.Message.Trim()
            };

            if (_env.IsDevelopment())
            {
                errorResult.Extensions.Add("stackTrace", exception.StackTrace!.Trim());
            }

            var properties = new Dictionary<string, object>
            {
                { "traceId", errorResult.Instance},
                { "StackTrace", exception.StackTrace!.Trim() },
                { "Source", exception.TargetSite?.DeclaringType?.FullName!}
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
