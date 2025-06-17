using Infrastructure.Data.DbContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Options;
using Infrastructure.Services.CQRS;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.DbContextDesignTime;

public class OrderDbContextDesignTime
    : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../PublicAPI");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        var masterConnectionString = configuration.GetValue<string>("DatabaseConnectionString:Postgresql:Master");
        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        return new OrderDbContext(
            optionsBuilder.Options,
            new DbConnectionStringSelector(
                Microsoft.Extensions.Options.Options.Create(new DatabaseConnectionStringOptions()
                {
                    Master = masterConnectionString
                })),
            new DispatcherDomainEventInterceptors(new EventDispatcher(null, null)
            ));
        optionsBuilder.UseNpgsql(masterConnectionString);
    }
}
