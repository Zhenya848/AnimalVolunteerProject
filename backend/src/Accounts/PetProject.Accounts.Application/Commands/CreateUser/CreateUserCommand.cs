namespace PetProject.Accounts.Application.Commands.CreateUser;

public record CreateUserCommand(string Email, string UserName, string Password);