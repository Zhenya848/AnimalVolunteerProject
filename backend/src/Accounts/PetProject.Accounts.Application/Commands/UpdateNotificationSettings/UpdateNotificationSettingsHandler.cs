using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Repositories;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Accounts.Application.Commands.UpdateNotificationSettings;

public class UpdateNotificationSettingsHandler
    : ICommandHandler<UpdateNotificationSettingsCommand, UnitResult<ErrorList>>
{
    private readonly INotificationProvider _notificationProvider;

    public UpdateNotificationSettingsHandler(
        INotificationProvider notificationProvider)
    {
        _notificationProvider = notificationProvider;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateNotificationSettingsCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await _notificationProvider
            .UpdateNotificationSettings(command.UserId, command.Email, command.Telegram, command.Web);

        if (result.IsFailure)
            return (ErrorList)result.Error;

        return Result.Success<ErrorList>();
    }
}