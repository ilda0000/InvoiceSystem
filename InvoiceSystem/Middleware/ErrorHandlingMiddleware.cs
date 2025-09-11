using System.Net;
using System.Text.Json;
using FluentValidation;
using InvoiceSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;

// Error handling middleware for catching and formatting exceptions in the HTTP pipeline
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
            var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList();

            // Create a response object including detailed errors
            var response = new
            {
                type = "ValidationError",
                status = StatusCodes.Status400BadRequest,
                message = "One or more validation errors occurred.",
                errors = errors
            };

            // Set the response HTTP status code
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            // Set the response content type to JSON
            context.Response.ContentType = "application/json";

            // Write the JSON response to the HTTP response body
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        //// Catch business exceptions
        catch (BusinessExceptions ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "BusinessError",
                status = StatusCodes.Status409Conflict,
                message = ex.Message
            }));
        }
       // Catch not found exceptions
        catch (NotFoundExceptions ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "NotFoundError",
                status = StatusCodes.Status404NotFound,
                message = ex.Message
            }));
        }
        //// Catch database exceptions
        catch (DatabaseException ex)
        {
            // Log the database error
            _logger.LogError(ex, "Database error");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "DatabaseError",
                status = StatusCodes.Status500InternalServerError,
                message = "A database error occurred. Please contact support."
            }));
        }
        //// Catch all other unhandled exceptions
        catch (Exception ex)
        {
            // Log the unexpected error as an error
            _logger.LogError(ex, "Unhandled server error");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "ServerError",
                status = StatusCodes.Status500InternalServerError,
                message = "An unexpected error occurred. Please try again later."
            }));
        }
    }
}
