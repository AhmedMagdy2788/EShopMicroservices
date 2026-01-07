using BuildingBlocks.HelperClasses;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}",
            exception.Message, DateTime.UtcNow);

        var (error, statusCode) = exception switch
        {
            ValidationException validationException => (
                Error.ValidationError(
                    validationException.Message,
                    new Dictionary<string, object?>
                    {
                        ["ValidationErrors"] = validationException.Errors,
                        ["traceId"] = httpContext.TraceIdentifier,
                        ["instance"] = httpContext.Request.Path.ToString()
                    }
                ),
                StatusCodes.Status400BadRequest
            ),
            BadRequestException => (
                Error.BadRequest(
                    exception.Message,
                    new Dictionary<string, object?>
                    {
                        ["traceId"] = httpContext.TraceIdentifier,
                        ["instance"] = httpContext.Request.Path.ToString()
                    }
                ),
                StatusCodes.Status400BadRequest
            ),
            NotFoundException => (
                Error.NotFound(
                    exception.Message,
                    new Dictionary<string, object?>
                    {
                        ["traceId"] = httpContext.TraceIdentifier,
                        ["instance"] = httpContext.Request.Path.ToString()
                    }
                ),
                StatusCodes.Status404NotFound
            ),
            _ => (
                Error.InternalServerError(
                    exception.Message,
                    new Dictionary<string, object?>
                    {
                        ["traceId"] = httpContext.TraceIdentifier,
                        ["instance"] = httpContext.Request.Path.ToString()
                    }
                ),
                StatusCodes.Status500InternalServerError
            )
        };

        httpContext.Response.StatusCode = statusCode;

        var result = Result<object>.Failure(error);

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}