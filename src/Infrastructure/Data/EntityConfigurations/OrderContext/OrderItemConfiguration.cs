using Core.BoundContext.OrderContext.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.OrderContext;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", DbSchema.OrderBook);
        builder.Property(x => x.BookName)
            .HasMaxLength(100);
        builder.Property(x=>x.Price)
            .HasColumnType("decimal(18,2)");
    }
}
