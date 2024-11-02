using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Accounts.Application;

public static class Inject
{
    public static IServiceCollection AddFromAccountsApplication(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
    }
}