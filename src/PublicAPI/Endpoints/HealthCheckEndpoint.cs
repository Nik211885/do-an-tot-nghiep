using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Payment;
using Application.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class HealthCheckEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var api = endpoint.MapGroup("health-check")
            .WithTags("HealthCheck");
        api.MapGet("health-app", ()=>TypedResults.Ok("Healthy"));
        api.MapGet("health-check-connection", async () =>
        {
            var configuration = endpoint.ServiceProvider.GetService<IConfiguration>();
            var connectionString = configuration!.GetValue<string>("DatabaseConnectionString:Postgresql:Master");
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                return TypedResults.Ok(new { status = "Healthy", db = "Connected", timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                return Results.Problem("Database connection failed: " + ex.Message);
            }
        });
        api.MapPost("health-get-token-exchange", async (
            [FromServices] IIdentityProviderServices identityService) =>
        {
            var token = await identityService.GetTokenAsync();
            return TypedResults.Ok(token);
        }).WithDescription("Get new token with client credentials grant");
        api.MapPost("momo", async (
            [FromServices] IPayment payment,
            [FromBody] PaymentRequest request) =>
        {
            var paymentResponse = await payment.CreatePaymentRequestAsync(request);
            return TypedResults.Ok(paymentResponse);
        });
    }
}
