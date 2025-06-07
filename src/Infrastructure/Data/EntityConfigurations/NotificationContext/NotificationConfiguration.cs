using Infrastructure.Services.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.NotificationContext;

public class NotificationConfiguration 
    : IEntityTypeConfiguration<Core.BoundContext.NotificationContext.Notification>
{
    public void Configure(EntityTypeBuilder<Core.BoundContext.NotificationContext.Notification> builder)
    {
        builder.ToTable("Notifications", DbSchema.Notification);
        builder.Property(n=>n.Message)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(n => n.Title)
            .HasMaxLength(100);
        builder.Property(n => n.NotificationChanel)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(n=>n.NotificationSubject)
            .HasConversion<string>()
            .HasMaxLength(100);
        builder.Property(n => n.Status)
            .HasConversion<string>()
            .HasMaxLength(100);
    }
}
