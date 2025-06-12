using Core.BoundContext.UserProfileContext.FollowerAggregate;
using Core.BoundContext.UserProfileContext.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.UserProfileContext;

public class FollowerConfiguration  
    : IEntityTypeConfiguration<Follower>
{
    public void Configure(EntityTypeBuilder<Follower> builder)
    {
        builder.ToTable("Follower", DbSchema.UserProfile);
    }
}
