using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared;
using PetProject.Infrastructure.Authentification;

namespace PetProject.Application.AccountManagement.Commands;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(UserManager<User> userManager, ILogger<CreateUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist != null)
            return (ErrorList)Errors.User.AlreadyExist();

        var user = new User()
        {
            Email = command.Email,
            UserName = command.UserName
        };
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        await _userManager.AddToRoleAsync(user, "Partisipant");
        
        _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);

        return Result.Success<ErrorList>();
    }
}