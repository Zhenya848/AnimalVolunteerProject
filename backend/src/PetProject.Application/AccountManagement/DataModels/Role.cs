using Microsoft.AspNetCore.Identity;

namespace PetProject.Infrastructure.Authentification;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}