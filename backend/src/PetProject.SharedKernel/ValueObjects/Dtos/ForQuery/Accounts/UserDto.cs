namespace PetProject.Core.ValueObjects.Dtos.ForQuery.Accounts;

public record UserDto()
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    
    public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; } = [];

    public ParticipantAccountDto? ParticipantAccount { get; set; }
    public VolunteerAccountDto? VolunteerAccount { get; set; }
    public AdminAccountDto? AdminAccount { get; set; }
}