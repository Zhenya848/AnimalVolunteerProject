namespace NotificationProject.Entities;

public record NotificationSettings
{
    public Guid UserId { get; set; }

    public bool Email { get; set; } = true;
    public bool Telegram { get; set; } = false;
    public bool Web { get; set; } = true;
}