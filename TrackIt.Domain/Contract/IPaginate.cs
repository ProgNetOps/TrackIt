using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.Contract
{
    public interface IPaginate<T>
    {
        Task<IEnumerable<T>> GetPagedAsync(int? pageNumber, int pageSize);
    }
}
