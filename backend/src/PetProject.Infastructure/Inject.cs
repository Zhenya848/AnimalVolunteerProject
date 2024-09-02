using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Application.Files.Providers;
using PetProject.Application.Repositories;
using PetProject.Infastructure.Options;
using PetProject.Infastructure.Providers;
using PetProject.Infastructure.Repositories;

namespace PetProject.Infastructure
{
    public static class Inject
    {
        public static IServiceCollection AddFromInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<IVolunteerRepository, VolunteerRepository>();
            services.AddMinio(configuration);

            return services;
        }

        private static IServiceCollection AddMinio(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMinio(o =>
            {
                MinioOptions minioOptions = configuration.GetSection("Minio").Get<MinioOptions>()
                ?? throw new ApplicationException("Missing minio configuration");

                o.WithEndpoint(minioOptions.Endpoint);
                o.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

                o.WithSSL(minioOptions.WithSsl);
            });

            services.AddScoped<IFileProvider, MinioProvider>();

            return services;
        }
    }
}
