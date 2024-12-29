namespace PetProject.Accounts.Presentation.Requests;

public record GetUsersRequest(
    IEnumerable<string> Users,
    IEnumerable<string> Roles);