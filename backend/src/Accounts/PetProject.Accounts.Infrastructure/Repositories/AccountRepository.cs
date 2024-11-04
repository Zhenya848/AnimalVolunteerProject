using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Application.Repositories;
using PetProject.Accounts.Domain.User;
using PetProject.Core;

namespace PetProject.Infrastructure.Authentification.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountsDbContext _accountsDbContext;

    public AccountRepository(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public Guid CreateParticipant(
        ParticipantAccount participantAccount)
    {
        var addResult = _accountsDbContext.ParticipantAccounts
            .Add(participantAccount);
        
        return participantAccount.Id;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken, 
        CancellationToken cancellationToken = default)
    {
        var refreshSession = await _accountsDbContext.RefreshSessions
            .Include(u => u.User)
            .FirstOrDefaultAsync(r => r.RefreshToken == refreshToken, cancellationToken);

        if (refreshSession == null)
            return Errors.General.NotFound(refreshToken);

        return refreshSession;
    }

    public void Delete(RefreshSession refreshSession)
    {
        _accountsDbContext.RefreshSessions.Remove(refreshSession);
    }
}