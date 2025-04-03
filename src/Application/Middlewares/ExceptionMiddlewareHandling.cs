using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Middlewares;
/// <summary>
/// 
/// </summary>
/// <param name="logger"></param>
public class ExceptionMiddlewareHandling(ILogger<ExceptionMiddlewareHandling> logger) : IMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ILogger<ExceptionMiddlewareHandling> _logger = logger;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <exception cref="NotImplementedException"></exception>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandlerExceptionAsync(context, ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    private async Task HandlerExceptionAsync(HttpContext context, Exception exception)
    {
        // var controllerName = context.GetRouteData()?.Values["controller"]?.ToString();
        // var actionName = context.GetRouteData()?.Values["action"]?.ToString();
        // var requestMethod = context.Request.Method;
        // var requestPath = context.Request.Path;
        // // change to utc + 7
        // var time = DateTimeOffset.UtcNow.AddHours(7);
        // var message =
        //     $"{time} - Exception occurred at {controllerName}/{actionName} - Path: {requestPath}, Method: {requestMethod}";
        var statusCode = GetStatusCode(exception);
        string title;
        string messageDetail;
        // change to utc + 7
        if (statusCode >= 500)
        {
            title = "Error";
            messageDetail = "Có lỗi ở server";
        }
        else
        {
            title = "Warning";
            messageDetail = exception.Message;
        }

        var response = (
            title, 
            statusCode, 
            messageDetail);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    private int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException=> StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
