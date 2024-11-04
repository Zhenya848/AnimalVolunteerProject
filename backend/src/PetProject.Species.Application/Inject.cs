using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Species.Application;

public static class Inject
{
    public static IServiceCollection AddFromSpeciesApplication(this IServiceCollection services)
    {
        var assembly = typeof(Inject).Assembly;
        
        return services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
    }
}