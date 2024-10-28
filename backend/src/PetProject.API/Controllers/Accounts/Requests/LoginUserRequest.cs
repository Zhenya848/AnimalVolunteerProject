namespace PetProject.API.Controllers.Accounts.Requests;

public record LoginUserRequest(string Email, string Password);