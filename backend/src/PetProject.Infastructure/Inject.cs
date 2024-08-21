using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Repositories;
using PetProject.Infastructure.Repositories;

namespace PetProject.Infastructure
{
    public static class Inject
    {
        public static IServiceCollection AddFromInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<IVolunteerRepository, VolunteerRepository>();

            return services;
        }
    }
}
