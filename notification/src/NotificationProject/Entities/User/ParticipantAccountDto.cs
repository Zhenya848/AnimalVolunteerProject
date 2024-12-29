namespace NotificationProject.Entities.User;

public record ParticipantAccountDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Patronymic { get; set; } = default!;
}