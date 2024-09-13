using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Contract;
using TrackIt.Domain;
using TrackIt.Repository.Base;
using System.Diagnostics;
using TrackIt.Repository.Services;
using TrackIt.Persistence;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TrackIt.Repository.Implementations;

public class NetworkSwitchService(AppDbContext context) : SQLBaseRepository<NetworkSwitch>(context), INetworkSwitchService
{

    public async Task<(IEnumerable<NetworkSwitch>?,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
    {
        IQueryable<NetworkSwitch> networkSwitches = from switches in context.NetworkSwitches.Include(q => q.IPPoP).
                                           ThenInclude(q => q.BTS).
                                           ThenInclude(q => q.State)
                                                    select switches;

        //Filtering                        
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals(nameof(NetworkSwitch.IPPoP.BTS.State.StateName), StringComparison.OrdinalIgnoreCase))
            {
                networkSwitches = networkSwitches.Where(q => q.IPPoP.BTS.State.StateName.Contains(filterQuery));
            }
            if (filterOn.Equals(nameof(NetworkSwitch.IPPoP.IPPoPName), StringComparison.OrdinalIgnoreCase))
            {
                networkSwitches = networkSwitches.Where(q => q.IPPoP.IPPoPName.Contains(filterQuery));
            }
        }


        //Sorting
        switch (sortBy)
        {
            case "state_desc":
                networkSwitches = networkSwitches.OrderByDescending(q => q.IPPoP.BTS.State.StateName);
                break;
            case "state":
                networkSwitches = networkSwitches.OrderBy(q => q.IPPoP.BTS.State.StateName);
                break;
            case "ippop_desc":
                networkSwitches = networkSwitches.OrderByDescending(q => q.IPPoP.IPPoPName);
                break;
            case "ippop":
                networkSwitches = networkSwitches.OrderBy(q => q.IPPoP.IPPoPName);
                break;

            //The default is ordering by name
            default:
                networkSwitches = networkSwitches.OrderBy(q => q.SwitchName);
                break;
        }

        pageNumber ??= 1;

        int count = networkSwitches.Count();

        networkSwitches = networkSwitches.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

        //Query only gets sent to the database from here.
        return (await networkSwitches.AsNoTracking().ToListAsync(),count);
    }

    public override async Task<NetworkSwitch?> GetByIdAsync(Guid id)
    {
        var networkSwitch = await context.NetworkSwitches.Include(q => q.IPPoP).
            ThenInclude(q => q.BTS).
            ThenInclude(q => q.State).
            ThenInclude(q => q.Zone)
            .FirstOrDefaultAsync(q => q.Id == id);

        return networkSwitch;
    }

   
}

    

