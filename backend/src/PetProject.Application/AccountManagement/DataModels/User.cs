using Microsoft.AspNetCore.Identity;

namespace PetProject.Infrastructure.Authentification;

public class User : IdentityUser<Guid>
{
    public List<SocialNetwork> SocialNetworkList { get; set; }
}