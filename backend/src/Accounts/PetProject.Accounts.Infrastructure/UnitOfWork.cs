using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Infrastructure.Authentification;

public class UnitOfWork : IUnitOfWork
{
    private readonly AccountsDbContext _appDbContext;

    public UnitOfWork(AccountsDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}