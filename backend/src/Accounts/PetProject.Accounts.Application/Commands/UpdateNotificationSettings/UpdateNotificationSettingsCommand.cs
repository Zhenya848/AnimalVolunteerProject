namespace PetProject.Accounts.Application.Commands.UpdateNotificationSettings;

public record UpdateNotificationSettingsCommand(
    Guid UserId,
    bool Email = true,
    bool Telegram = false,
    bool Web = true);