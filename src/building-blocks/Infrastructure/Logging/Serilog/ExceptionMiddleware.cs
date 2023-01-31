using System.Net;
using FSH.Core.Common;
using FSH.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;

namespace FSH.Infrastructure.Logging.Serilog;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ISerializationService _jsonSerializer;

    public ExceptionMiddleware(ISerializationService jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace.Trim());
            LogContext.PushProperty("ErrorMessage", exception.Message.Trim());
            LogContext.PushProperty("ErrorType", exception.GetType().FullName);
            var errorResult = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Type = exception.GetType().FullName,
                Title = exception.GetType().Name,
                Detail = exception.Message.Trim(),
                Instance = errorId
            };
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            errorResult.Status = exception switch
            {
                CustomException e => (int)e.StatusCode,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int?)(int)HttpStatusCode.InternalServerError,
            };
            Log.Error($"Request failed with Status Code {errorResult.Status} and Error Id {errorId}.");
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = (int)errorResult.Status;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}
