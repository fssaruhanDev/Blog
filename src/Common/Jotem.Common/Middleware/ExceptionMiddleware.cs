using Jotem.Infrastructure.Persistence.Exeptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jotem.Common.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Sonraki middleware veya endpoint'e geç
            await _next(context);
        }
        catch (Exception ex)
        {
            // Yakalanmayan exception var ise burada yakalarız
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Varsayılan olarak 500
        var statusCode = HttpStatusCode.InternalServerError;
        var errorMessage = "Internal Server Error";

        if (exception is DatabaseValidationException dbEx)
        {
            // Exception sınıfından aldığın StatusCode’u kullanabilirsin
            statusCode = (HttpStatusCode)(dbEx.StatusCode);
            errorMessage = dbEx.ErrorMessage;
        }

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = errorMessage
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var payload = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(payload);
    }
}