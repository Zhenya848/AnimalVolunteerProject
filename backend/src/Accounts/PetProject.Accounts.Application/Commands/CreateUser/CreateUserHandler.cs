using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Domain.User;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Accounts.Application.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(
        UserManager<User> userManager, 
        ILogger<CreateUserHandler> logger)
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
        
        var user = User.Create(command.UserName, command.Email);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        await _userManager.AddToRoleAsync(user, "Volunteer");
        
        _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);

        return Result.Success<ErrorList>();
    }
}