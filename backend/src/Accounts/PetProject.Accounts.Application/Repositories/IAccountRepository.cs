using CSharpFunctionalExtensions;
using PetProject.Accounts.Domain.User;
using PetProject.Core;

namespace PetProject.Accounts.Application.Repositories;

public interface IAccountRepository
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken,
        CancellationToken cancellationToken = default);
    
    void Delete(RefreshSession refreshSession);
}