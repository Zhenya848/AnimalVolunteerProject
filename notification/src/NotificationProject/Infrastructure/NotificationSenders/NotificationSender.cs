using NotificationProject.Infrastructure.NotificationSenders.Abstractions;

namespace NotificationProject.Infrastructure.NotificationSenders;

public class NotificationSender : INotificationSender
{
    public void SendNotification(ISender sender) => sender.Send();
}