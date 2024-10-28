namespace PetProject.API.Controllers.Accounts.Requests;

public record CreateUserRequest(string Email, string UserName, string Password);