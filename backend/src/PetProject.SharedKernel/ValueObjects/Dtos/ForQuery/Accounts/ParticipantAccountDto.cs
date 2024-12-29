namespace PetProject.Core.ValueObjects.Dtos.ForQuery.Accounts;

public record ParticipantAccountDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Patronymic { get; set; } = default!;
}