using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;
using Core.BoundContext.UserProfileContext.FollowerAggregate;
using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;
using Core.BoundContext.UserProfileContext.UserProfileAggregate;
using Infrastructure.Data.EntityConfigurations.UserProfileContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class UserProfileDbContext(DbContextOptions<UserProfileDbContext> options, 
    IDbConnectionStringSelector dbConnectionStringSelector, DispatcherDomainEventInterceptors interceptors)
    : BaseDbContext(options, dbConnectionStringSelector, interceptors)
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<FavoriteBook> FavoriteBooks { get; set; }
    public DbSet<Follower> Followers { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration())
            .ApplyConfiguration(new FavoriteBookConfiguration())
            .ApplyConfiguration(new FollowerConfiguration())
            .ApplyConfiguration(new SearchHistoryConfiguration());
    }
}
