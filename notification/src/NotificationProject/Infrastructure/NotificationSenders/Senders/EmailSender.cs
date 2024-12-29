using NotificationProject.Entities;
using NotificationProject.Entities.User;
using NotificationProject.Infrastructure.NotificationSenders.Abstractions;

namespace NotificationProject.Infrastructure.NotificationSenders.Senders;

public class EmailSender : ISender
{
    public string Email { get; set; }
    public string Message { get; set; }
    
    public void Send()
    {
        Console.WriteLine($"Sending message: {Message} to email: {Email}");
    }
}