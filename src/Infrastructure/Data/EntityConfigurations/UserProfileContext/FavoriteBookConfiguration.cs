using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.UserProfileContext;

public class FavoriteBookConfiguration
    : IEntityTypeConfiguration<FavoriteBook>
{
    public void Configure(EntityTypeBuilder<FavoriteBook> builder)
    {
        builder.ToTable("FavoriteBook" , DbSchema.UserProfile);
    }
}
