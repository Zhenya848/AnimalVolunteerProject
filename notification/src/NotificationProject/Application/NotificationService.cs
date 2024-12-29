using Microsoft.EntityFrameworkCore;
using NotificationProject.Entities;
using NotificationProject.Infrastructure.DbContexts;
using NotificationService;

namespace NotificationProject.Application;

public class NotificationService
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        NotificationDbContext notificationDbContext,
        ILogger<NotificationService> logger)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
    }

    public async Task UpdateNotificationSettingsHandle(UpdateNotificationSettingsRequest request)
    {
        try
        {
            var userId = Guid.Parse(request.UserId);

            var settingsExist = await _notificationDbContext.NotificationSettings
                .Where(i => i.UserId == userId).FirstOrDefaultAsync();

            if (settingsExist != null)
            {
                settingsExist.Email = request.Email;
                settingsExist.Telegram = request.Telegram;
                settingsExist.Web = request.Web;

                _notificationDbContext.NotificationSettings.Attach(settingsExist);
            }
            else
            {
                var notificationService = new NotificationSettings() 
                    { UserId = userId, Email = request.Email, Telegram = request.Telegram, Web = request.Web };

                _notificationDbContext.NotificationSettings.Add(notificationService);
            }

            await _notificationDbContext.SaveChangesAsync();

            _logger.LogInformation("Notification settings updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while updating the notification settings.");
        }
    }

    public async Task SendNotificationHandle(SendNotificationRequest request)
    {
        try
        {
            var notification = new Notification()
            {
                Id = Guid.NewGuid(),
                Message = request.Message,
                Roles = request.Roles.ToList(),
                Users = request.Users.ToList(),
                IsSent = false
            };

            _notificationDbContext.Notifications.Add(notification);
            
            await _notificationDbContext.SaveChangesAsync();
            
            _logger.LogInformation("Notification sent successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while sending the notification.");
        }
    }
}