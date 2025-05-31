using System.Reflection;
using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.BoundContext.BookAuthoringContext.IntegrationEvent.EventHandler;
using Application.Interfaces.EventBus;
using Application.Models.EventBus;
using Infrastructure.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.EventBus;

public static class AddMassTransitRabbitMqEventBusExtension
{
    public static IServiceCollection AddMassTransitRabbitMqEventBus(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEventBus<>), typeof(MassTransitEventBus<>));
        services.AddScoped(typeof(IIntegrationEventHandler<>), typeof(MassTransitIntegrationEventHandler<>));

        var eventTypes = typeof(IntegrationEvent).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(IntegrationEvent)))
            .ToList();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            foreach (var eventType in eventTypes)
            {
                var consumerType = typeof(MassTransitIntegrationEventHandler<>).MakeGenericType(eventType);
                x.AddConsumer(consumerType);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });

                foreach (var eventType in eventTypes)
                {
                    var consumerType = typeof(MassTransitIntegrationEventHandler<>).MakeGenericType(eventType);
                    cfg.ReceiveEndpoint($"event-{eventType.Name}", ep =>
                    {
                        ep.ConfigureConsumer(context, consumerType);
                    });
                }
            });
        });

        return services;
    }
}
