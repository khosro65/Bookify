
using Bookify.Application.Abstractions.Exceptions;
using Bookify.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Middleware;

public class GlobalExceptionHandlingMiddleware 
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {

            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var exceptionDetails = GetExceptionDetails(exception);

            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }

            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                Status: StatusCodes.Status400BadRequest,
                Type: "ValidationFailure",
                Title: "Validation Error",
                Detail: "One or more validation errors has occurred",
                Errors: validationException.Errors
                ),
           
            _ => new ExceptionDetails(
                Status: StatusCodes.Status400BadRequest,
                Type: "ServerError",
                Title: "Server error",
                Detail: "An unexpected error has occurred",
                Errors: null
                ),
        };
    }

    internal record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors);
}
