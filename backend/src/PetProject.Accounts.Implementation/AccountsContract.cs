using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Contracts;
using PetProject.Infrastructure.Authentification;

namespace PetProject.Accounts.Implementation;

public class AccountsContract : IAccountsContract
{
    private readonly AccountsDbContext _accountsDbContext;

    public AccountsContract(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }
    
    public async Task<IReadOnlyList<string>> GetUserPermissionCodes(Guid userId)
    {
        var permissions = await _accountsDbContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        return permissions;
    }
}