using Application.Interfaces.IdentityProvider;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class WeatherForecast : IEndpoints
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("api/weather-forecast");
        apis.MapGet("users/{id}", async (string id,
            [FromServices] IIdentityProviderServices identityServices) 
            => Results.Ok(await identityServices.GetUserInfoAsync(id)));
        apis.MapGet("/", GetWeatherForecast)
            .Produces<WeatherForecastRecord[]>(200);
    }
    [Authorize]
    public static IResult GetWeatherForecast()
    {
        var forecast =  Enumerable.Range(1, 5).Select(index =>
                new WeatherForecastRecord
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray();
        return TypedResults.Ok<WeatherForecastRecord[]>(forecast);
    }
}
record WeatherForecastRecord(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
