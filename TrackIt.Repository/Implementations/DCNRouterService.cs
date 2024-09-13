using TrackIt.Domain;
using TrackIt.Repository.Base;
using TrackIt.Repository.Services;
using TrackIt.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TrackIt.Repository.Implementations;

  public class DCNRouterService(AppDbContext context) : SQLBaseRepository<DCNRouter>(context), IDCNRouterService
  {      
  

    public async Task<(IEnumerable<DCNRouter>,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
      {
          IQueryable<DCNRouter> dcnRouters = from sites in context.DCNRouters.Include(q => q.IPPoP).
                                             ThenInclude(q => q.BTS).
                                             ThenInclude(q => q.State)
                                                select sites;


        //Filtering                        
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals(nameof(DCNRouter.RouterName), StringComparison.OrdinalIgnoreCase))
            {
                dcnRouters = dcnRouters.Where(q => q.RouterName.Contains(filterQuery));
            }
            else if (filterOn.Equals(nameof(DCNRouter.IPPoP), StringComparison.OrdinalIgnoreCase))
            {
                dcnRouters = dcnRouters.Where(q => q.IPPoP.IPPoPName.Contains(filterQuery));
            }
            else if (filterOn.Equals(nameof(DCNRouter.RouterType), StringComparison.OrdinalIgnoreCase))
            {
                dcnRouters = dcnRouters.Where(q => q.RouterType.Contains(filterQuery));
            }
        }

        //Sorting
        switch (sortBy)
          {
                case "name_desc":
                dcnRouters = dcnRouters.OrderByDescending(q => q.RouterName);
                break;
            case "name":
                dcnRouters = dcnRouters.OrderBy(q => q.RouterName);
                break;
            case "state_desc":
                dcnRouters = dcnRouters.OrderByDescending(q => q.IPPoP.BTS.State.StateName);
                  break;
              case "state":
                dcnRouters = dcnRouters.OrderBy(q => q.IPPoP.BTS.State.StateName);
                  break;
              case "ippop_desc":
                dcnRouters = dcnRouters.OrderByDescending(q => q.IPPoP.IPPoPName);
                  break;
              case "ippop":
                dcnRouters = dcnRouters.OrderBy(q => q.IPPoP.IPPoPName);
                  break;
     
              //The default is ordering by name
              default:
                dcnRouters = dcnRouters.OrderBy(q => q.RouterName);
                  break;
          }
     
          pageNumber ??= 1;

        int count = dcnRouters.Count();

        dcnRouters = dcnRouters.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);
     
          //Query only gets sent to the database from here.
          return (await dcnRouters.AsNoTracking().ToListAsync(), count);
      }
     
  }

