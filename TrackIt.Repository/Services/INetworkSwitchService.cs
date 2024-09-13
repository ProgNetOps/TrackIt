using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Contract;
using TrackIt.Domain;
using TrackIt.Repository.Base;

namespace TrackIt.Repository.Services
{
    public interface INetworkSwitchService : IBaseRepository<NetworkSwitch>, IFilterSortPaginate<NetworkSwitch>
    {

    }
}
