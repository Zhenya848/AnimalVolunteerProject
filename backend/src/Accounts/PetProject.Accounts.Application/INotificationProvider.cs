using CSharpFunctionalExtensions;
using PetProject.Core;

namespace PetProject.Accounts.Application;

public interface INotificationProvider
{
    public Task<UnitResult<Error>> SendNotification(string message, string[] roles, string[] users);

    public Task<UnitResult<Error>> UpdateNotificationSettings(
        Guid userId,
        bool email = true,
        bool telegram = false,
        bool web = true);
}