using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.Contract
{
    public interface ISearchable<T>
    {
        Task<IEnumerable<T>?> GetFilteredResultAsync(string filterOn, string filterQuery, int pageNumber, int pageSize);
    }
}
