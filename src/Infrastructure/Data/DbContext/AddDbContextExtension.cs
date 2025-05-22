using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.DbContext;

internal static class AddDbContextExtension
{
    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ModerationDbContext>();
        services.AddDbContext<BookReviewDbContext>();
        services.AddDbContext<BookAuthoringDbContext>();
        services.AddDbContext<OrderDbContext>();
        services.AddDbContext<ReportSubmissionDbContext>();
        return services;
    } 
}
