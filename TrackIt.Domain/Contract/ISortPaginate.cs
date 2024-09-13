using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Enums;

namespace TrackIt.Domain.Contract
{
    public interface ISortPaginate<T>
    {
        Task<IEnumerable<T>?> GetSortedPagesAsync(string sortBy, int? pageNumber, int pageSize);
        //Task<IEnumerable<T>?> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize);
    }
}
