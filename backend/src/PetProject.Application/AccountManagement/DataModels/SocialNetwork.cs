namespace PetProject.Infrastructure.Authentification;

public record SocialNetwork
{
    public string Name { get; } = default!;
    public string Reference { get; } = default!;

    private SocialNetwork(string name, string reference)
    {
        Name = name;
        Reference = reference;
    }

    private SocialNetwork() { }
};