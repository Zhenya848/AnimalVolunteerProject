namespace PetProject.Accounts.Presentation.Requests;

public record CreateUserRequest(string Email, string UserName, string Password);