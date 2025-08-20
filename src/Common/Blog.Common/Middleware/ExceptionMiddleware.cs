using Blog.Common.Infrastructure.Exeptions;
using Blog.Infrastructure.Persistence.Exeptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.Common.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode = StatusCodes.Status500InternalServerError;
        string type = "https://example.com/probs/internal-server-error";
        string title = "Internal Server Error";
        string detail = "An unexpected error occurred.";
        IDictionary<string, string[]>? errors = null;

        switch (exception)
        {
            case ApiException apiEx:
                statusCode = apiEx.StatusCode;
                type = apiEx.Type ?? type;
                title = apiEx.Title ?? title;
                detail = apiEx.Message;
                errors = apiEx.Errors;
                break;
            case DatabaseValidationException dbEx:
                statusCode = dbEx.StatusCode;
                type = "https://example.com/probs/database-validation";
                title = "Database Validation Error";
                detail = dbEx.ErrorMessage;
                break;
            case ValidationException fvEx:
                statusCode = StatusCodes.Status400BadRequest;
                type = "https://example.com/probs/validation";
                title = "One or more validation errors occurred.";
                detail = fvEx.Message;
                errors = fvEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                break;
            default:
                // keep defaults
                break;
        }

        var problem = new
        {
            type,
            title,
            status = statusCode,
            detail,
            instance = context.Request.Path.Value,
            errors
        };

        _logger.LogError(exception, "An exception was thrown while processing {Method} {Path}", context.Request.Method, context.Request.Path);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;
        var payload = JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(payload);
    }
}