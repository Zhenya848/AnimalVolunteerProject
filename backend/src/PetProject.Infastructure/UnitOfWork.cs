using Microsoft.EntityFrameworkCore.Storage;
using PetProject.Application.Database;
using PetProject.Infastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infastructure
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
