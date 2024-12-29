namespace PetProject.Accounts.Application.Commands.GetUsers;

public record GetUsersCommand(
    IEnumerable<string> Users,
    IEnumerable<string> Roles);