using System.Net;
using System.Text.Json;
using FluentValidation;
using static InvoiceSystem.ErrorMessages.PlanError;
using Microsoft.AspNetCore.Mvc;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next; // reference to the next middleware in the pipeline 
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) //constructor to initialize the middleware with the next delegate and logger
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context) //main method called for every http request
    {
        try
        {
            await _next(context); //call the next middleware in the pipeline
        }
        catch (ValidationException ex) // catch validation exceptions from FluentValidation 
        {
            _logger.LogWarning(ex, "Validation failed"); //log the validation as a warning 
            await WriteProblem(context,
                StatusCodes.Status400BadRequest, //httpcode 400 for bad request
                "Validation error", ex.Message);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, int status, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
