using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Repositories;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Accounts.Application.Commands.SendNotification;

public class SendNotificationHandler : ICommandHandler<SendNotificationCommand, UnitResult<ErrorList>>
{
    private readonly INotificationProvider _notificationProvider;
    private readonly IAccountRepository _accountRepository;

    public SendNotificationHandler(
        INotificationProvider notificationProvider,
        IAccountRepository accountRepository)
    {
        _notificationProvider = notificationProvider;
        _accountRepository = accountRepository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        SendNotificationCommand command, 
        CancellationToken cancellationToken = default)
    {
        var usersExist = await _accountRepository
            .GetUsers(command.Users, command.Roles, cancellationToken);
        
        if (usersExist.Any() == false)
            return (ErrorList)Error
                .NotFound("users.not.found", "Users with these names or roles are not found");
        
        var result = await _notificationProvider
            .SendNotification(command.Message, command.Roles.ToArray(), command.Users.ToArray());
        
        if (result.IsFailure)
            return (ErrorList)result.Error;

        return Result.Success<ErrorList>();
    }
}