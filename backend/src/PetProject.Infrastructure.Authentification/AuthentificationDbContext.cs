using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PetProject.Infrastructure.Authentification;

public class AuthentificationDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().Property(sn => sn.SocialNetworkList).HasConversion(
            s => JsonSerializer.Serialize(s, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<List<SocialNetwork>>(json, JsonSerializerOptions.Default)!);
        
        modelBuilder.Entity<Role>().ToTable("roles");
        
        modelBuilder.Entity<Permission>().ToTable("permissions");
        modelBuilder.Entity<Permission>().HasIndex(c => c.Code).IsUnique();
        modelBuilder.Entity<Permission>().Property(d => d.Description).HasMaxLength(200);
        
        modelBuilder.Entity<RolePermission>().ToTable("role_permissions");
        modelBuilder.Entity<RolePermission>().HasOne<Role>().WithMany(rp => rp.RolePermissions)
            .HasForeignKey(i => i.RoleId);
        modelBuilder.Entity<RolePermission>().HasOne<Permission>().WithMany()
            .HasForeignKey(i => i.PermissionId);
        modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
        
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}