using NotificationProject.Infrastructure.NotificationSenders.Abstractions;

namespace NotificationProject.Infrastructure.NotificationSenders.Senders;

public class WebSender : ISender
{
    public string Message { get; set; }
    
    public void Send()
    {
        Console.WriteLine($"Sending message: {Message} to frontend");
    }
}