using TrackIt.Domain.Contract;
using TrackIt.Domain;
using TrackIt.Repository.Base;
using TrackIt.Domain.ViewModel;

namespace TrackIt.Repository.Services;
 public interface ICircuitService : IBaseRepository<Circuit>, IFilterSortPaginate<Circuit>
{
   Task<(Circuit circuit, Guid id)> AddCircuitAsync(Circuit entity);
    Task<CircuitCreateDropdownViewModel> GetNewCircuitDropdownValues();
    Circuit? UpdateCircuit(Circuit circuitChanges);
	Task<Circuit?> GetCircuitDetails(Guid id);
}



