namespace PetProject.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken);