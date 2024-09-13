using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Contract;
using TrackIt.Domain;
using TrackIt.Repository.Base;
using TrackIt.Domain.ViewModel;

namespace TrackIt.Repository.Services;
public interface IClientService: IBaseRepository<Client>, IFilterSortPaginate<Client>
{
    //Task<IEnumerable<Client>> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize);
    Task<(Client client, Guid id)> AddClientAsync(Client entity);
    Task<ClientCreateDropdownViewModel> GetNewClientDropdownValues();

}
