using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.DbContext;

internal static class AddDbContextExtension
{
    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<WriteBookDbContext>();
        services.AddDbContext<BookReviewDbContext>();
        return services;
    } 
}
