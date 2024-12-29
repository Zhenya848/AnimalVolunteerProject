using NotificationProject.Infrastructure.DbContexts;
using NotificationProject.Application;
using NotificationProject.Infrastructure.BackgroundServices;
using NotificationProject.Infrastructure.NotificationSenders;
using NotificationProject.Infrastructure.NotificationSenders.Abstractions;
using NotificationProject.Infrastructure.NotificationSenders.Senders;
using NotificationProject.Services;

namespace NotificationProject;

public static class Inject
{
    public static IServiceCollection AddFromNotificationService(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<INotificationSender, NotificationSender>();
        
        services.AddScoped<NotificationDbContext>();
        services.AddScoped<Application.NotificationService>();

        services.AddHttpClient();
        services.AddSingleton<AccountService>();

        services.AddHostedService<SendNotificationsBackgroundService>();
        
        return services;
    }
}