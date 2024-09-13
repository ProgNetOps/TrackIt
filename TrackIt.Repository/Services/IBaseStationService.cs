using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Contract;
using TrackIt.Domain;
using TrackIt.Repository.Base;
using TrackIt.Domain.ViewModel;

namespace TrackIt.Repository.Services
{
    public interface IBaseStationService : IBaseRepository<BTS>, IFilterSortPaginate<BTS>
    {
        Task<IEnumerable<BTS>> GetSamplePdf();
        Task<NewBTSDropdownsVM> GetNewBTSDropdownValues();
       Task<(BTS baseStation, Guid id)> AddBTSAsync(BTS entity);
        Task<BTS?> GetBTSDetails(Guid id);
        BTS? UpdateBTS(BTS btsChanges);
    }
}
