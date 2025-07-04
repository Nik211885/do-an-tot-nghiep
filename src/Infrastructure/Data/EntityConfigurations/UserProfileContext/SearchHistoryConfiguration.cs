using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;
using Core.BoundContext.UserProfileContext.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.UserProfileContext;

public class SearchHistoryConfiguration 
    : IEntityTypeConfiguration<SearchHistory>
{
    public void Configure(EntityTypeBuilder<SearchHistory> builder)
    {
        builder.ToTable( "SearchHistory",DbSchema.UserProfile);
        builder.Property(x=>x.SearchTerm)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(x=>x.IpAddress)
            .HasMaxLength(50);
    }
}
