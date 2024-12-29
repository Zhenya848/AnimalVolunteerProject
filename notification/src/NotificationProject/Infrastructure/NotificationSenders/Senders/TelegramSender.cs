using NotificationProject.Infrastructure.NotificationSenders.Abstractions;

namespace NotificationProject.Infrastructure.NotificationSenders.Senders;

public class TelegramSender : ISender
{
    public string UserName { get; set; }
    public string Message { get; set; }
    
    public void Send()
    {
        Console.WriteLine($"Sending message: {Message} to User: {UserName}");
    }
}