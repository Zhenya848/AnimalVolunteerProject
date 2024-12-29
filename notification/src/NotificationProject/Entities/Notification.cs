namespace NotificationProject.Entities;

public record Notification
{
    public Guid Id { get; set; }
    
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> Users { get; set; } = new List<string>();
    public string Message { get; set; }
    
    public bool IsSent { get; set; }
};