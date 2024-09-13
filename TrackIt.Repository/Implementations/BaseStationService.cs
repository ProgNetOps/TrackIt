using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Persistence;
using TrackIt.Repository.Base;
using TrackIt.Repository.Services;

namespace TrackIt.Repository.Implementations;

/// <summary>
/// A repository class for BTS related database interactions
/// </summary>
/// <param name="context"></param>
    public class BaseStationService(AppDbContext context) : SQLBaseRepository<BTS>(context), IBaseStationService
    {    

        private DbSet<BTS> allBTS => context.BaseStations;

    /// <summary>
    /// Creates a new BaseStation 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>A valuetuple containing the newly created base station and its Id value</returns>
    public async Task<(BTS baseStation,Guid id)> AddBTSAsync(BTS entity)
    {
        await allBTS.AddAsync(entity);
        var newId = entity.Id;
        await context.SaveChangesAsync();

        return (entity,newId);
    }
    
    public async Task<BTS?> GetBTSDetails(Guid id)
    {
        var baseStation = await allBTS.Include(q => q.State).
            AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
        return baseStation;
    }

    public BTS? UpdateBTS(BTS btsChanges)
    {
        var bts = allBTS.Attach(btsChanges);
        bts.State = EntityState.Modified;
        context.SaveChanges();
        return btsChanges;
    }

    public async Task<IEnumerable<BTS>> GetSamplePdf()
        {
            return await allBTS.OrderBy(q => q.BTSName).
            Take(30).AsNoTracking().
            ToListAsync();
        }

    
    public async Task<(IEnumerable<BTS>?,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery,string sortBy, int? pageNumber, int pageSize)
    {
        IQueryable<BTS> baseStations = from sites in allBTS.Include(q => q.State) select sites;
        
        //Filtering                        
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals(nameof(BTS.BTSName), StringComparison.OrdinalIgnoreCase))
            {
                baseStations = baseStations.Where(q => q.BTSName.Contains(filterQuery));
            }
            else if (filterOn.Equals(nameof(BTS.LocationAddress), StringComparison.OrdinalIgnoreCase))
            {
                baseStations = baseStations.Where(q => q.LocationAddress.Contains(filterQuery));
            }
            else if (filterOn.Equals(nameof(BTS.State.StateName), StringComparison.OrdinalIgnoreCase))
            {
                baseStations = baseStations.Where(q => q.State.StateName.Contains(filterQuery));
            }
        }

        //Sorting
        switch (sortBy)
        {
            case "name_desc":
                baseStations = baseStations.OrderByDescending(q => q.BTSName);
                break;
            case "name":
                baseStations = baseStations.OrderBy(q => q.BTSName);
                break;
            case "state_desc":
                baseStations = baseStations.OrderByDescending(q => q.State.StateName);
                break;
            case "state":
                baseStations = baseStations.OrderBy(q => q.State.StateName);
                break;

            //The default is ordering by name
            default:
                baseStations = baseStations.OrderBy(q => q.BTSName);
                break;
        }

        pageNumber ??= 1;


        int count = baseStations.Count();

        baseStations = baseStations.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

        //Query only gets sent to the database from here.
        return (await baseStations.AsNoTracking().ToListAsync(), count);
    }

    public async Task<NewBTSDropdownsVM> GetNewBTSDropdownValues()
        {
            var response = new NewBTSDropdownsVM()
            {
                States = await context.States.
                OrderBy(q => q.StateName).
                AsNoTracking().
                ToListAsync()
            };

            return response;
        }

    
    }


