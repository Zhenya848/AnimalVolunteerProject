namespace NotificationProject.Infrastructure.NotificationSenders.Abstractions;

public interface INotificationSender
{
    public void SendNotification(ISender sender);
}