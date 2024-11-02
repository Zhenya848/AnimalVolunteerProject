using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;

namespace PetProject.Core.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddFromCoreInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

            return services;
        }
    }
}
