namespace PetProject.Infrastructure.Authentification.Options;

public class RefreshSessionOptions
{
    public const string RefreshSession = "RefreshSession";
    
    public int ExpiredDaysTime { get; init; }
}