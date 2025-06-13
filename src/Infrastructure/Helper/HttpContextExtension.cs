using Microsoft.AspNetCore.Http;

namespace Infrastructure.Helper;

public static class HttpContextExtension
{
    public static string GetIpAddress(this HttpContext context)
    {
        string? ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress?.ToString();
        }
        if (ip == "::1")
        {
            ip = "127.0.0.1";
        }
    
        return ip ?? string.Empty;

    }
}
