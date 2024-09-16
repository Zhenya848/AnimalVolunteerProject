using System.Data;

namespace PetProject.Application.Database
{
    public interface IUnitOfWork
    {
        Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

        Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
