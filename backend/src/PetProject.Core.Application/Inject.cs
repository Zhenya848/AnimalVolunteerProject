using Microsoft.Extensions.DependencyInjection;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Core.Application
{
    public static class Inject
    {
        public static IServiceCollection AddFromCoreApplication(this IServiceCollection services)
        {
            services.AddHandlers();

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
                .AddClasses(classes => classes.AssignableToAny(
                    typeof(ICommandHandler<,>),
                    typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithLifetime(ServiceLifetime.Scoped));
        }
    }
}
