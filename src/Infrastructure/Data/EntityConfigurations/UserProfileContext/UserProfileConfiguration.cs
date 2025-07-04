using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;
using Core.BoundContext.UserProfileContext.FollowerAggregate;
using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;
using Core.BoundContext.UserProfileContext.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.UserProfileContext;

public class UserProfileConfiguration
    : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfile",DbSchema.UserProfile);
        builder.Property(x => x.Bio)
            .HasMaxLength(500);
        builder.HasMany<SearchHistory>()
            .WithOne()
            .HasForeignKey(x => x.UserId);
        builder.HasMany<Follower>()
            .WithOne()
            .HasForeignKey(x => x.FollowerId);
        builder.HasMany<Follower>()
            .WithOne()
            .HasForeignKey(x => x.FollowingId);
        builder.HasMany<FavoriteBook>()
            .WithOne()
            .HasForeignKey(x => x.UserId);
    }
}
