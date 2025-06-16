using Application.Interfaces.Payment;
using Infrastructure.Helper;
using Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Payment;

public static class AddPaymentServicesExtension
{
    public static IServiceCollection AddPaymentService(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientKeyFactory.BankMomoKey,
            (sp, client) =>
            {
                var momoOptionsConfig = sp.GetRequiredService<IOptions<MomoConfigOptions>>();
                string paymentUrl = momoOptionsConfig.Value.PaymentUrl;  
                client.BaseAddress = new Uri(paymentUrl);
            });
        services.AddScoped<IPayment, MomoGatewayPayment>();
        return services;
    }
}
