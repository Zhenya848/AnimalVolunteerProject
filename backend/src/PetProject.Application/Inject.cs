using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;

namespace PetProject.Application
{
    public static class Inject
    {
        public static IServiceCollection AddFromApplication(this IServiceCollection services)
        {
            services.AddScoped<IVolunteerService, VolunteerService>();

            return services;
        }
    }
}
