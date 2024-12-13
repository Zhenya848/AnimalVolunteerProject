using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Volunteers.Application;

public static class Inject
{
    public static IServiceCollection AddFromVolunteersApplication(this IServiceCollection services)
    {
        var assembly = typeof(Inject).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
        
        return services.AddValidatorsFromAssembly(assembly);
    }
}