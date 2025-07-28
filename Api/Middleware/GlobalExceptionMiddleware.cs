using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using Flight.Api.Models;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;


namespace Flight.Api.Middleware;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException validationEx => new ErrorResponse
            {
                Title = "Validation Error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = "One or more validation errors occurred",
                Errors = validationEx.Errors
            },
            
            UnauthorizedAccessException => new ErrorResponse
            {
                Title = "Unauthorized",
                Status = (int)HttpStatusCode.Unauthorized,
                Detail = "Authentication is required to access this resource"
            },
            
            ForbiddenAccessException forbiddenEx => new ErrorResponse
            {
                Title = "Forbidden",
                Status = (int)HttpStatusCode.Forbidden,
                Detail = forbiddenEx.Message
            },
            
            _ => new ErrorResponse
            {
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred"
            }
        };

        context.Response.StatusCode = response.Status;

        // Log different levels based on exception type
        switch (exception)
        {
            case ValidationException:
                logger.LogWarning(exception, "Client error occurred: {ExceptionType}", exception.GetType().Name);
                break;
            
            case UnauthorizedAccessException:
            case ForbiddenAccessException:
                logger.LogWarning(exception, "Security exception occurred for user {UserId}: {ExceptionType}", 
                    context.User?.Identity?.Name ?? "Anonymous", exception.GetType().Name);
                break;
            
            default:
                logger.LogError(exception, "Server error occurred: {ExceptionType}", exception.GetType().Name);
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
