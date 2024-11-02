namespace PetProject.Accounts.Domain.User;

public record SocialNetwork
{
    public string Name { get; } = default!;
    public string Reference { get; } = default!;

    public SocialNetwork(string name, string reference)
    {
        Name = name;
        Reference = reference;
    }

    private SocialNetwork() { }
};