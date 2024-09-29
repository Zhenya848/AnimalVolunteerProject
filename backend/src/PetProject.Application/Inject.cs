using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Files.Services;
using PetProject.Application.Volunteers.Queries.Pets.Services;
using PetProject.Application.Volunteers.UseCases.Pets.Services;
using PetProject.Application.Volunteers.UseCases.Services;

namespace PetProject.Application
{
    public static class Inject
    {
        public static IServiceCollection AddFromApplication(this IServiceCollection services)
        {
            services.AddScoped<IVolunteerService, VolunteerService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPetQueryService, PetQueryService>();

            return services;
        }
    }
}
