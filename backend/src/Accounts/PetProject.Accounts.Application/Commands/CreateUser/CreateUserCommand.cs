using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Accounts.Application.Commands.CreateUser;

public record CreateUserCommand(
    string Email, 
    string UserName, 
    FullNameDto FullName,
    string Password,
    IEnumerable<SocialNetworkDto> SocialNetworks);