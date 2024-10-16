﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Application;
using PetProject.Application.Database;
using PetProject.Application.Files.Providers;
using PetProject.Application.Messaging;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Repositories.Write;
using PetProject.Infastructure.BackgroundServices;
using PetProject.Infastructure.DbContexts;
using PetProject.Infastructure.MessageQueues;
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
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IReadDbContext, ReadDbContext>();
            services.AddScoped<WriteDbContext>();

            services.AddScoped<IVolunteerRepository, VolunteerRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMinio(configuration);
            services.AddHostedService<FileCleanerBackgroundService>();

            services.AddSingleton<IMessageQueue<IEnumerable<Application.Files.Providers.FileInfo>>, 
                InMemoryMessageQueue<IEnumerable<Application.Files.Providers.FileInfo>>>();

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
