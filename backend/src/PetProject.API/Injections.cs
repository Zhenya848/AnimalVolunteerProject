using FluentValidation;

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