
namespace TrackIt.Domain.Contract
{
    public interface IFilterSortPaginate<T>
    {
        Task<(IEnumerable<T>?,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize);

    }
}
