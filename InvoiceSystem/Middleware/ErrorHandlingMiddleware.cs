using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public class ErrorHandlingMiddleware
{
    // Reference to the next middleware in the pipeline
    private readonly RequestDelegate _next;

    // Logger to log exceptions and warnings
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    // Constructor: initializes the middleware with the next delegate and logger
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Main method called for every HTTP request
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
        // Catch validation exceptions thrown by FluentValidation
        catch (ValidationException ex)
        {
            // Log the validation warning
            _logger.LogWarning(ex, "Validation failed");

            // Extract all error messages from the validation exception
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

            // Create a ProblemDetails object describing the error
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest, // HTTP 400 for validation errors
                Title = "Validation error",               // Error title
                Detail = "One or more validation errors occurred." // General detail message
            };

            // Create a response object including detailed errors
            var response = new
            {
                problem.Status,
                problem.Title,
                problem.Detail,
                Errors = errors
            };

            // Set the response HTTP status code
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            // Set the response content type to JSON
            context.Response.ContentType = "application/json";

            // Write the JSON response to the HTTP response body
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        // Catch all other unhandled exceptions
        catch (Exception ex)
        {
            // Log the unexpected error as an error
            _logger.LogError(ex, "Unexpected error");

            // Return a generic 500 Internal Server Error response
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.");
        }
    }

    // Helper method to write ProblemDetails responses
    private static async Task WriteProblem(HttpContext ctx, int status, string title, string detail)
    {
        // Create a ProblemDetails object
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };

        // Set the response HTTP status code
        ctx.Response.StatusCode = status;

        // Set the response content type to JSON
        ctx.Response.ContentType = "application/json";

        // Write the JSON response to the HTTP response body
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
