using Microsoft.AspNetCore.Identity;

namespace PetProject.Accounts.Domain.User;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}