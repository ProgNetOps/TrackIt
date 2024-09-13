using Microsoft.EntityFrameworkCore;
using TrackIt.Domain;
using TrackIt.Persistence;
using TrackIt.Repository.Base;
using TrackIt.Repository.Services;

namespace TrackIt.Repository.Implementations
{
    public class IPPoPService(AppDbContext context) : SQLBaseRepository<IPPoP>(context), IIPPoPService
    {

        public async override Task<IPPoP?> GetByIdAsync(Guid id)
        {
            var ipPoP = await context.IPPoPs.
                Include(q => q.Routers).
                Include(q => q.Switches).
                Include(q => q.Circuits).
                Include(q => q.BTS).
                ThenInclude(q => q.State)
                .FirstOrDefaultAsync(p => p.Id == id);

            return ipPoP;
        }


        public async Task<(IEnumerable<IPPoP>?, int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable<IPPoP> IPPoPs = from pops in context.IPPoPs.Include(q => q.BTS).
                                       ThenInclude(q => q.State).
                                       Include(q => q.Switches).
                                       Include(q => q.Circuits).
                                       Include(q => q.Routers)
                                       select pops;
            //Filtering                        
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals(nameof(IPPoP.IPPoPName), StringComparison.OrdinalIgnoreCase))
                {
                    IPPoPs = IPPoPs.Where(q => q.IPPoPName.Contains(filterQuery));
                }
                if (filterOn.Equals(nameof(IPPoP.BTS.State.StateName), StringComparison.OrdinalIgnoreCase))
                {
                    IPPoPs = IPPoPs.Where(q => q.BTS.State.StateName.Contains(filterQuery));
                }
            }


            //Sorting
            switch (sortBy)
            {
                case "name_desc":
                    IPPoPs = IPPoPs.OrderByDescending(q => q.IPPoPName);
                break;
                case "name":
                    IPPoPs = IPPoPs.OrderBy(q => q.IPPoPName);
                break;
                case "state_desc":
                    IPPoPs = IPPoPs.OrderByDescending(q => q.BTS.State.StateName);
                break;
                case "state":
                    IPPoPs = IPPoPs.OrderBy(q => q.BTS.State.StateName);
                break;

                //The default is ordering by name
                default:
                    IPPoPs = IPPoPs.OrderBy(q => q.IPPoPName);
                break;
            }

            pageNumber ??= 1;

            int count = IPPoPs.Count();

            IPPoPs = IPPoPs.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            //Query only gets sent to the database from here.
            return (await IPPoPs.AsNoTracking().ToListAsync(),count);
        }
    }
}
