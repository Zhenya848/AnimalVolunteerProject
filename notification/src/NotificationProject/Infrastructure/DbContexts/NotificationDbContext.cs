using Microsoft.EntityFrameworkCore;
using NotificationProject.Entities;

namespace NotificationProject.Infrastructure.DbContexts;

public class NotificationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<NotificationSettings> NotificationSettings { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}