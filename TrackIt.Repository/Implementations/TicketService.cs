
using Microsoft.EntityFrameworkCore;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Persistence;
using TrackIt.Repository.Base;
using TrackIt.Repository.Services;

namespace TrackIt.Repository.Implementations
{
    public class TicketService(AppDbContext context) : SQLBaseRepository<Ticket>(context), ITicketService
    {
        public async Task<(Ticket ticket, Guid id)> AddTicketAsync(Ticket entity)
        {
            await context.Tickets.AddAsync(entity);
            var newId = entity.Id;
            await context.SaveChangesAsync();

            return (entity, newId);
        }


        public async Task<(IEnumerable<Ticket>?, int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable<Ticket> tickets = from troubletickets in context.Tickets.
                                         Include(q => q.TicketStatus).                                       
                                       Include(q => q.TicketType).
                                       Include(q => q.TicketPriority).
                                       Include(q => q.Circuit)
                                       select troubletickets;
            //Filtering                        
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals(nameof(Ticket.Circuit.Client.ClientName), StringComparison.OrdinalIgnoreCase))
                {
                    tickets = tickets.Where(q => q.Circuit.Client.ClientName.Contains(filterQuery));
                }
            }


            //Sorting
            switch (sortBy)
            {
                case "logTime_desc":
                    tickets = tickets.OrderByDescending(q => q.LoggedAt);
                    break;
                case "logTime":
                    tickets = tickets.OrderBy(q => q.LoggedAt);
                    break;
                case "state_desc":
                    tickets = tickets.OrderByDescending(q => q.Circuit.State.StateName);
                    break;
                case "state":
                    tickets = tickets.OrderBy(q => q.Circuit.State.StateName);
                    break;
                case "zone_desc":
                    tickets = tickets.OrderByDescending(q => q.Circuit.State.Zone.ZoneName);
                    break;
                case "zone":
                    tickets = tickets.OrderBy(q => q.Circuit.State.Zone.ZoneName);
                    break;

                //The default is ordering by logTime
                default:
                    tickets = tickets.OrderBy(q => q.LoggedAt);
                    break;
            }

            pageNumber ??= 1;

            int count = tickets.Count();

            tickets = tickets.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            //Query only gets sent to the database from here.
            return (await tickets.AsNoTracking().ToListAsync(), count);

        }

        public async Task<NewTicketDropdownsVM> GetNewTicketDropdownValues(Client client)
        {
            var response = new NewTicketDropdownsVM(client)
            {
                Clients = await context.Clients.
                OrderBy(q => q.ClientName).
                AsNoTracking().
                ToListAsync(),

                Circuits = await context.Circuits.
                Where(q => q.ClientId == client.Id).
                AsNoTracking().
                ToListAsync(),

                TicketTypes = await context.TicketTypes.
                OrderBy(q => q.TypeOfTicket).
                AsNoTracking().
                ToListAsync(), 

                TicketStatuses = await context.TicketStatuses.
                OrderBy(q => q.Status).
                AsNoTracking().
                ToListAsync(),

                TicketPriorities = await context.TicketPriorities.
                OrderBy(q => q.Priority).
                AsNoTracking().
                ToListAsync(),
            };

            return response;
        }

        public Task<IEnumerable<Ticket>> GetSamplePdf()
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> GetTicketDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public Ticket? UpdateTicket(Ticket ticketChanges)
        {
            throw new NotImplementedException();
        }
    }
}
