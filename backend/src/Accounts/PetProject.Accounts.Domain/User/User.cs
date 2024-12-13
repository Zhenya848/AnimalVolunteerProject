using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetProject.Core;
using PetProject.Core.ValueObjects;
using PetProject.Core.ValueObjects.Dtos;

namespace PetProject.Accounts.Domain.User;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];
    private List<SocialNetwork> _socialNetworks = [];
    
    public IReadOnlyList<Role> Roles => _roles;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    private User()
    {
        
    }
    
    private static User Create(
        string user, 
        string email, 
        IEnumerable<SocialNetwork> socialNetworks, 
        Role role)
    {
        return new User
        {
            UserName = user,
            Email = email,
            _socialNetworks = socialNetworks.ToList(),
            _roles =  [role]
        };
    }

    public static User CreateParticipant(
        string user,
        string email,
        IEnumerable<SocialNetwork> socialNetworks,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, socialNetworks, role);
    }
    
    public static User CreateAdmin(
        string user,
        string email,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, [], role);
    }
}