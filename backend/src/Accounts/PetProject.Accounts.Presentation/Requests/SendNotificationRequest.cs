namespace PetProject.Accounts.Presentation.Requests;

public record SendNotificationRequest(
    IEnumerable<string> Users,
    IEnumerable<string> Roles,
    string Message);