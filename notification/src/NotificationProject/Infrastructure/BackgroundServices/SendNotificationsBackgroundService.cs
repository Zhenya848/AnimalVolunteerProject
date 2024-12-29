using Microsoft.EntityFrameworkCore;
using NotificationProject.Infrastructure.DbContexts;
using NotificationProject.Infrastructure.NotificationSenders.Abstractions;
using NotificationProject.Infrastructure.NotificationSenders.Senders;
using NotificationProject.Services;
using NotificationProject.Services.Requests;

namespace NotificationProject.Infrastructure.BackgroundServices;

public class SendNotificationsBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly INotificationSender _notificationSender;
    private readonly AccountService _accountService;

    public SendNotificationsBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        INotificationSender notificationSender,
        AccountService accountService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _notificationSender = notificationSender;
        _accountService = accountService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var notificationDbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            
            await Task.Delay(
                TimeSpan.FromSeconds(30),
                stoppingToken);
            
            var notifications = await notificationDbContext.Notifications
                .Where(n => n.IsSent == false)
                .ToListAsync(stoppingToken);

            foreach (var notification in notifications)
            {
                var usersExist = await _accountService
                    .GetUsers(new GetUsersRequest(notification.Users, notification.Roles), stoppingToken);

                var usersWithNotificationSettings = await notificationDbContext.NotificationSettings
                    .Where(i => usersExist.Select(x => x.Id).Contains(i.UserId))
                    .ToDictionaryAsync(i => i, i => usersExist
                        .First(x => x.Id == i.UserId), stoppingToken);
                
                foreach (var (settings, user) in usersWithNotificationSettings)
                {
                    if (settings.Email)
                    {
                        var emailSender = new EmailSender() 
                            { Email = user.Email, Message = notification.Message };

                        _notificationSender.SendNotification(emailSender);
                    }
                    
                    if (settings.Telegram)
                    {
                        var telegramSender = new TelegramSender()
                            { Message = notification.Message, UserName = string.Empty };
                        
                        _notificationSender.SendNotification(telegramSender);
                    }

                    if (settings.Web)
                    {
                        var webSender = new WebSender()
                            { Message = notification.Message };
                        
                        _notificationSender.SendNotification(webSender);
                    }
                }
                
                notification.IsSent = true;
                
                await Task.Delay(
                    TimeSpan.FromSeconds(1),
                    stoppingToken);
            }

            await notificationDbContext.SaveChangesAsync(stoppingToken);
        }
    }
}