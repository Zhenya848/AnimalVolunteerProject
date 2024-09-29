using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Domain.Species;
using PetProject.Domain.Volunteers;
using PetProject.Infastructure.Interceptors;

namespace PetProject.Infastructure.DbContexts
{
    public class WriteDbContext(IConfiguration configuration) : DbContext
    {
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Species> Species { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(WriteDbContext).Assembly, 
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
