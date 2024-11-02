namespace PetProject.Accounts.Contracts;

public interface IAccountsContract
{
    Task<IReadOnlyList<string>> GetUserPermissionCodes(Guid userId);
}