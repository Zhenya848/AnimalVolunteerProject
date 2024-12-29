namespace PetProject.Accounts.Presentation.Requests;

public record UpdateNotificationSettingsRequest(
    Guid UserId,
    bool Email = true,
    bool Telegram = false,
    bool Web = true);