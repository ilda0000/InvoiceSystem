using System.Net;
using System.Text.Json;
using FluentValidation;
using InvoiceSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed");

            var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList();

            var response = new
            {
                type = nameof(ValidationException), // Shows actual exception name
                status = StatusCodes.Status400BadRequest,
                message = "One or more validation errors occurred.",
                errors = errors
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (BusinessExceptions ex)
        {
            _logger.LogWarning(ex, "Business exception occurred");

            await WriteErrorResponse(context, StatusCodes.Status409Conflict, ex);
        }
        catch (NotFoundExceptions ex)
        {
            _logger.LogWarning(ex, "Not found exception occurred");

            await WriteErrorResponse(context, StatusCodes.Status404NotFound, ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Database error");

            await WriteErrorResponse(context, StatusCodes.Status500InternalServerError, ex,
                "A database error occurred. Please contact support.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled server error");

            await WriteErrorResponse(context, StatusCodes.Status500InternalServerError, ex,
                "An unexpected error occurred. Please try again later.");
        }
    }

    private async Task WriteErrorResponse(HttpContext context, int statusCode, Exception ex, string overrideMessage = null)
    {
        var response = new
        {
            type = ex.GetType().Name,   // Shows exact exception type
            status = statusCode,
            message = overrideMessage ?? ex.Message
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
