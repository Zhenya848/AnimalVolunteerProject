namespace PetProject.Core.ValueObjects.Dtos.ForQuery.Accounts;

public record VolunteerAccountDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Patronymic { get; set; } = default!;
    
    public RequisiteDto[] Requisites { get; set; } = [];
    public int Expirience { get; set; }
}