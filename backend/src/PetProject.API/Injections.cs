using FluentValidation;
using FluentValidation.AspNetCore;
using PetProject.Application;

namespace PetProject.API
{
    public static class Inject
    {
        public static IServiceCollection AddFromAPI(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}