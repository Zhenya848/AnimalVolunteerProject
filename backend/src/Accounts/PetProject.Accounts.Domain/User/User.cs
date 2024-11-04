using Microsoft.AspNetCore.Identity;
using PetProject.Core;

namespace PetProject.Accounts.Domain.User;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];
    public IReadOnlyList<Role> Roles => _roles;

    private User()
    {
        
    }
    
    public static User Create(string user, string email, Role? role = null)
    {
        return new User
        {
            UserName = user,
            Email = email,
            _roles = role == null ? [] : [role]
        };
    }
}