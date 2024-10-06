using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Files;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.Commands;
using PetProject.Application.Volunteers.Pets.Commands;
using PetProject.Application.Volunteers.Pets.Commands.UploadPhotos;
using PetProject.Application.Volunteers.Pets.Queries;

namespace PetProject.Application
{
    public static class Inject
    {
        public static IServiceCollection AddFromApplication(this IServiceCollection services)
        {
            services.AddHandlers();
            services.AddScoped<UploadFilesToPetHandler>();

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes.AssignableToAny(
                    typeof(ICreateHandler<,>),
                    typeof(IDeleteHandler<,>),
                    typeof(IUpdateHandler<,>),
                    typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithLifetime(ServiceLifetime.Scoped));
        }
    }
}
