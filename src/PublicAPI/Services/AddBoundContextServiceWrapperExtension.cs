using PublicAPI.Endpoints;

namespace PublicAPI.Services;

public static class AddBoundContextServiceWrapperExtension
{
    public static IServiceCollection AddServiceWrapper(this IServiceCollection services)
    {
        services.AddScoped<BookAuthoringServiceWrapper>();
        return services;
    }
}
