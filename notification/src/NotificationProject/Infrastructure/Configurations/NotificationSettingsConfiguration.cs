using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationProject.Entities;

namespace NotificationProject.Infrastructure.Configurations;

public class NotificationSettingsConfiguration : IEntityTypeConfiguration<NotificationSettings>
{
    public void Configure(EntityTypeBuilder<NotificationSettings> builder)
    {
        builder.ToTable("notification_settings");

        builder.HasKey(i => i.UserId);
        
        builder.Property(e => e.Email).IsRequired();
        builder.Property(t => t.Telegram).IsRequired();
        builder.Property(w => w.Web).IsRequired();
    }
}