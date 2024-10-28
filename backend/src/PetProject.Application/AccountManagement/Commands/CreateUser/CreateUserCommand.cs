namespace PetProject.Application.AccountManagement.Commands;

public record CreateUserCommand(string Email, string UserName, string Password);