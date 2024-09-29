namespace PetProject.Application.Volunteers
{
    public class PagedList<T>
    {
        public IReadOnlyList<T> Items { get; init; } = default!;

        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }

        public bool HasNextPage => TotalCount < (Page - 1) * PageSize;
        public bool HasPreviousPage => Page > 1;
    }
}
