using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.DbContext;

internal static class AddDbContextExtension
{
    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ModerationDbContext>();
        services.AddDbContext<BookAuthoringDbContext>();
        services.AddDbContext<BookAuthoringDbContext>();
        services.AddDbContext<OrderDbContext>();
        return services;
    } 
}
