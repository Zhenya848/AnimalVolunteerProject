using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Species.Application.Repositories;
using PetProject.Species.Infrastructure.DbContexts;
using PetProject.Species.Infrastructure.Repositories;

namespace PetProject.Species.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromSpeciesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }
}