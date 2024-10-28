namespace PetProject.Application.Shared.Interfaces;

public interface IQueryHandler<TQuery, TResult>
{
    public Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}