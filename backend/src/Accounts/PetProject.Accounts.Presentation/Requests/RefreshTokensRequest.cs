namespace PetProject.Accounts.Presentation.Requests;

public record RefreshTokensRequest(string AccessToken, Guid RefreshToken);