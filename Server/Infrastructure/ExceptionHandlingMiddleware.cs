using Microsoft.AspNetCore.Mvc;
using Server.Domain.Exceptions;

namespace Server.Infrastructure;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка при обработке запроса: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Bad Request (Validation Error)"),
            
            InvalidOperationDomainException => (StatusCodes.Status400BadRequest, "Invalid Operation"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid Operation"),
            
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),

            NetworkCommunicationException or HttpRequestException 
                => (StatusCodes.Status502BadGateway, "Network/External Service Error"),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error / Service Unavailable")
        };

        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}