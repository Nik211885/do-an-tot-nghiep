using Core.BoundContext.OrderContext.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.OrderContext;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", DbSchema.OrderBook);
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(x => x.OrderCode)
            .HasMaxLength(50);
    }
}
