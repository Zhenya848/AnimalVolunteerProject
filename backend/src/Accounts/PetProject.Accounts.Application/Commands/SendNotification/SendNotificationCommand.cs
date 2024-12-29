namespace PetProject.Accounts.Application.Commands.SendNotification;

public record SendNotificationCommand(
    IEnumerable<string> Users,
    IEnumerable<string> Roles,
    string Message);