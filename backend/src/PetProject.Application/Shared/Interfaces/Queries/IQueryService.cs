namespace PetProject.Application.Shared.Interfaces.Queries
{
    public interface IQueryService<TResponse, TQuery> where TQuery : IQuery
    {
        public Task<TResponse> Get(
            TQuery query,
            CancellationToken cancellationToken = default);
    }
}
