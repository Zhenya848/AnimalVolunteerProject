namespace NotificationProject.Services.Requests;

public record GetUsersRequest(
    IEnumerable<string> Users,
    IEnumerable<string> Roles);