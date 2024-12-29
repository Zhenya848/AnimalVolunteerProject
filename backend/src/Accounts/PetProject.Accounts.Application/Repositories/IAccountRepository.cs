using CSharpFunctionalExtensions;
using PetProject.Accounts.Domain.User;
using PetProject.Core;

namespace PetProject.Accounts.Application.Repositories;

public interface IAccountRepository
{
    public Guid CreateParticipant(
        ParticipantAccount participantAccount);
    
    Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken,
        CancellationToken cancellationToken = default);
    
    void Delete(RefreshSession refreshSession);
    
    public Task<Result<User, Error>> GetInfoAboutUser(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<User>> GetUsers(
        IEnumerable<string> users,
        IEnumerable<string> roles,
        CancellationToken cancellationToken = default);
}