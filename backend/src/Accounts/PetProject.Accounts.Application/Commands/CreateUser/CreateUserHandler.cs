using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Application.Repositories;
using PetProject.Accounts.Domain.User;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.ValueObjects;

namespace PetProject.Accounts.Application.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager,
        IAccountRepository accountRepository,
        ILogger<CreateUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _accountRepository = accountRepository;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist != null)
            return (ErrorList)Errors.User.AlreadyExist();

        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT)
            ?? throw new ApplicationException($"Role {ParticipantAccount.PARTICIPANT} does not exist");
        
        var socialNetworks = command.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Name, s.Reference).Value).ToList();
        
        var user = User.CreateParticipant(command.UserName, command.Email, socialNetworks, role);
        
        var fullName = FullName.Create(
            command.FullName.firstName,
            command.FullName.lastName,
            command.FullName.patronymic ?? "").Value;
        
        var participantAccount = ParticipantAccount.CreateParticipant(fullName, user);

        _accountRepository.CreateParticipant(participantAccount);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);

        return Result.Success<ErrorList>();
    }
}