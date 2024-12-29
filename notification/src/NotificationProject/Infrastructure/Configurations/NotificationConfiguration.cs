using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationProject.Entities;

namespace NotificationProject.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        
        builder.HasKey(x => x.Id);

        builder.Property(r => r.Roles).HasConversion(
            valueComparer => JsonSerializer.Serialize(valueComparer, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb");
        
        builder.Property(u => u.Users).HasConversion(
            valueComparer => JsonSerializer.Serialize(valueComparer, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb");
        
        builder.Property(m => m.Message).IsRequired();
        builder.Property(i => i.IsSent).IsRequired();
    }
}