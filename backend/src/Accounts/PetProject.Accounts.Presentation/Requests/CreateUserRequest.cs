using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Accounts.Presentation.Requests;

public record CreateUserRequest(
    string Email, 
    string UserName, 
    FullNameDto FullName,
    string Password, 
    IEnumerable<SocialNetworkDto> SocialNetworks);