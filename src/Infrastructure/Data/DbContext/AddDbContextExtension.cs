using System.Reflection;
using Core.Interfaces;
using Infrastructure.Options;
using Infrastructure.Services.DbContext;
using Infrastructure.Services.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.DbContext;

internal static class AddDbContextExtension
{
    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ModerationDbContext>();
        services.AddDbContext<BookReviewDbContext>();
        services.AddDbContext<BookAuthoringDbContext>();
        services.AddDbContext<OrderDbContext>();
        /*services.AddDbContext<ReportSubmissionDbContext>();*/
        services.AddDbContext<NotificationDbContext>();
        services.AddDbContext<UserProfileDbContext>();
        using var scope = services.BuildServiceProvider().CreateScope();    
        DatabaseConnectionStringOptions databaseConnectionStringOptions
            =  scope.ServiceProvider.GetRequiredService<IOptions<DatabaseConnectionStringOptions>>().Value;
        IDbConnectionStringSelector dbConnectionStringSelector
            = scope.ServiceProvider.GetRequiredService<IDbConnectionStringSelector>();
        Migrations<ModerationDbContext>();
        Migrations<BookReviewDbContext>();
        Migrations<BookAuthoringDbContext>();
        Migrations<OrderDbContext>();
        Migrations<NotificationDbContext>();
        Migrations<UserProfileDbContext>();
        void Migrations<TDbContext>() where TDbContext : BaseDbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseNpgsql(databaseConnectionStringOptions.Master);
            var dbContext = (TDbContext)Activator.CreateInstance(
                typeof(TDbContext),
                optionsBuilder.Options,
                dbConnectionStringSelector,
                null
            )!;
            var migrations = dbContext.Database.GetMigrations();
            if(!migrations.Any())
            {
                dbContext.Database.Migrate();
            }
        };
        return services;
    } 
}
