
using TrackIt.Domain;
using TrackIt.Domain.Contract;
using TrackIt.Domain.ViewModel;
using TrackIt.Repository.Base;

namespace TrackIt.Repository.Services;

public interface ITicketService:IBaseRepository<Ticket>, IFilterSortPaginate<Ticket>
{
    Task<IEnumerable<Ticket>> GetSamplePdf();
    Task<NewTicketDropdownsVM> GetNewTicketDropdownValues(Client client);
    Task<(Ticket ticket, Guid id)> AddTicketAsync(Ticket entity);
    Task<Ticket?> GetTicketDetails(Guid id);
    Ticket? UpdateTicket(Ticket ticketChanges);
}
