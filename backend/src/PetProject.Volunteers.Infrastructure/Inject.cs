using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Messaging;
using PetProject.Core.Application.Repositories;
using PetProject.Core.Infrastructure.DbContexts;
using PetProject.Core.Infrastructure.MessageQueues;
using PetProject.Volunteers.Application.Providers;
using PetProject.Volunteers.Application.Volunteers.Repositories;
using PetProject.Volunteers.Infrastructure.BackgroundServices;
using PetProject.Volunteers.Infrastructure.DbContexts;
using PetProject.Volunteers.Infrastructure.Options;
using PetProject.Volunteers.Infrastructure.Providers;
using PetProject.Volunteers.Infrastructure.Repositories;
using FileInfo = PetProject.Volunteers.Application.Providers.FileInfo;

namespace PetProject.Volunteers.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromVolunteersInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddMinio(configuration);
        services.AddHostedService<FileCleanerBackgroundService>();
        
        services.AddScoped<IFileProvider, MinioProvider>();
        
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, 
            InMemoryMessageQueue<IEnumerable<FileInfo>>>();

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

        return services;
    }
}