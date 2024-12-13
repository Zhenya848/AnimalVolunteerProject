using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetProject.Core.Application.Abstractions;
using PetProject.Species.Infrastructure.DbContexts;

namespace PetProject.Species.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WriteDbContext _appDbContext;

        public UnitOfWork(WriteDbContext appDbContext)
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
}
