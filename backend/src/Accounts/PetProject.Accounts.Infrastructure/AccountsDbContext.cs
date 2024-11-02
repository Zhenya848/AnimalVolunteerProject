using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Domain.User;
using PetProject.Core;

namespace PetProject.Infrastructure.Authentification;

public class AccountsDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    
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
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        modelBuilder.Entity<AdminAccount>().ToTable("admin_accounts");
        modelBuilder.Entity<AdminAccount>().HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(i => i.UserId);
        modelBuilder.Entity<AdminAccount>().ComplexProperty(fn => fn.FullName, fnb =>
        {
            fnb.Property(f => f.FirstName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH).HasColumnName("first_name");
            fnb.Property(l => l.LastName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH).HasColumnName("last_name");
            fnb.Property(p => p.Patronymic).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH).HasColumnName("patronymic");
        });
        
        modelBuilder.Entity<Role>().ToTable("roles");
        
        modelBuilder.Entity<Permission>().ToTable("permissions");
        modelBuilder.Entity<Permission>().HasIndex(c => c.Code).IsUnique();
        modelBuilder.Entity<Permission>().Property(d => d.Description).HasMaxLength(200);
        
        modelBuilder.Entity<RolePermission>().ToTable("role_permissions");
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.Entity<RefreshSession>().ToTable("refresh_sessions");
        
        modelBuilder.Entity<RefreshSession>()
            .HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(i => i.UserId);

        modelBuilder.Entity<RefreshSession>().HasKey(i => i.Id);
        
        modelBuilder.Entity<RefreshSession>().Property(j => j.Jti);
        modelBuilder.Entity<RefreshSession>().Property(c => c.CreatedAt);
        modelBuilder.Entity<RefreshSession>().Property(e => e.ExpiresIn);
        
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");

        //modelBuilder.HasDefaultSchema("accounts");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}