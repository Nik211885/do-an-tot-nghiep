using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Core.Exception;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Exceptions;

/// <summary>
/// Middleware xử lý exception với logging và error handling được cải thiện.
/// </summary>
public class ExceptionMiddlewareHandling(ILogger<ExceptionMiddlewareHandling> logger) : IMiddleware
{
    private readonly ILogger<ExceptionMiddlewareHandling> _logger = logger;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await LogExceptionAsync(context, ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task LogExceptionAsync(HttpContext context, Exception exception)
    {
        try
        {
            var logLevel = GetLogLevel(exception);
            var requestInfo = await GetRequestInfoAsync(context);
            
            _logger.Log(logLevel, exception, 
                "[{StatusCode}] {Method} {Path} - {ExceptionType}: {Message}. Request: {RequestInfo}",
                GetStatusCode(exception),
                context.Request.Method,
                context.Request.Path,
                exception.GetType().Name,
                exception.Message,
                requestInfo);
        }
        catch (Exception logEx)
        {
            _logger.LogError(logEx, "Failed to log exception details");
            _logger.LogError(exception, "Original exception occurred");
        }
    }

    private async Task<string> GetRequestInfoAsync(HttpContext context)
    {
        try
        {
            if (context.Request.HasFormContentType)
            {
                var form = await context.Request.ReadFormAsync();
                var formData = form.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToString()
                );
                return JsonSerializer.Serialize(formData, _jsonOptions);
            }
            
            if (context.Request.ContentLength is > 0 and < 1024 * 10) 
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                return body;
            }
            
            return $"Content-Length: {context.Request.ContentLength ?? 0}";
        }
        catch
        {
            return "Unable to read request body";
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Response has already started, cannot modify response for exception handling");
            return;
        }

        var statusCode = GetStatusCode(exception);
        var response = CreateErrorResponse(exception, statusCode);

        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = statusCode;
        
        var jsonResponse = JsonSerializer.Serialize(response, _jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }

    private object CreateErrorResponse(Exception exception, int statusCode)
    {
        var title = GetErrorTitle(statusCode);
        var traceId = System.Diagnostics.Activity.Current?.Id ?? Guid.NewGuid().ToString();
        object error = exception switch
        {
            ValidationException validationException => validationException
                .Errors.Select(errors => new { field = errors.PropertyName, message = errors.ErrorMessage, }).ToList(),
            BadRequestException badRequestException => badRequestException.Message,
            PermissionDeniedException  userForbiddenException => userForbiddenException.Message,
            NotFoundException notFoundException => notFoundException.Message,
            _ => "Có lỗi server trong quá trình xử lý"
        };
        return new
        {
            title, status = statusCode, traceId, error,
        };
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        BadRequestException => StatusCodes.Status400BadRequest,
        ValidationException => StatusCodes.Status400BadRequest,
        InvalidOperationException => StatusCodes.Status409Conflict,
        NotFoundException => StatusCodes.Status404NotFound,
        PermissionDeniedException => StatusCodes.Status403Forbidden,
        NotImplementedException => StatusCodes.Status501NotImplemented,
        TimeoutException => StatusCodes.Status408RequestTimeout,
        _ => StatusCodes.Status500InternalServerError
    };

    private static LogLevel GetLogLevel(Exception exception) => exception switch
    {
        BadRequestException => LogLevel.Warning,
        ValidationException => LogLevel.Warning,
        NotFoundException => LogLevel.Warning,
        _ => LogLevel.Error
    };

    private static string GetErrorTitle(int statusCode) => statusCode switch
    {
        >= 400 and < 500 => "Client Error",
        >= 500 => "Server Error",
        _ => "Error"
    };
}
