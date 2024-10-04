using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetProject.Application.Repositories.Read;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infastructure.DbContexts
{
    public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(WriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}