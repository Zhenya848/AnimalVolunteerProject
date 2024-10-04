using Microsoft.Extensions.DependencyInjection;
using PetProject.Application.Files;
using PetProject.Application.Shared.Interfaces.Commands;
using PetProject.Application.Shared.Interfaces.Queries;
using PetProject.Application.Volunteers.Commands;
using PetProject.Application.Volunteers.Pets.Commands;
using PetProject.Application.Volunteers.Pets.Queries;

namespace PetProject.Application
{
    public static class Inject
    {
        public static IServiceCollection AddFromApplication(this IServiceCollection services)
        {
            services.AddCommands().AddQueries();

            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(ICommandService<,,,>)))
                .AsSelfWithInterfaces()
                .WithLifetime(ServiceLifetime.Scoped));
        }
        private static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes.AssignableToAny(typeof(IQueryService<,>)))
                .AsSelfWithInterfaces()
                .WithLifetime(ServiceLifetime.Scoped));
        }
    }
}
