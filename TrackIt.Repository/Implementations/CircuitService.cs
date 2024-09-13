using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Persistence;
using TrackIt.Repository.Base;
using TrackIt.Repository.Services;

namespace TrackIt.Repository.Implementations
{
    public class CircuitService(AppDbContext context) : SQLBaseRepository<Circuit>(context), ICircuitService
    {
        public async Task<(Circuit circuit, Guid id)> AddCircuitAsync(Circuit entity)
        {
            await context.Circuits.AddAsync(entity);
            var newId = entity.Id;
            await context.SaveChangesAsync();

            return (entity, newId);
        }

		public Circuit? UpdateCircuit(Circuit circuitChanges)
		{
			var circuit = context.Circuits?.Attach(circuitChanges);
			circuit.State = EntityState.Modified;
			context.SaveChanges();
			return circuitChanges;
		}

		public async Task<Circuit?> GetCircuitDetails(Guid id)
        {
            var circuit = await context.Circuits.
                              Include(q => q.Client).
                              Include(q => q.CircuitState).
                              Include(q => q.LastMileDevice).
                              Include(q => q.State).
                              Include(q => q.IPPoP).
                              Include(q => q.Service).
                              AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
            return circuit;
        }

        public async Task<(IEnumerable<Circuit>,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable<Circuit> circuits = from services in context.Circuits.
                                     Include(q => q.Client).
                                     Include(q => q.CircuitState).
                                     Include(q => q.LastMileDevice).
                                     Include(q => q.State).
                                     ThenInclude(q => q.Zone).
                                     ThenInclude(q => q.TechnicalRegion).
                                     Include(q => q.IPPoP).
                                     Include(q => q.Service).
                                     Where(q => q.CircuitState.Name != "Terminated").
                                     OrderBy(q => q.CircuitName)
                                     select services;

            //Filtering                        
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals(nameof(Circuit.CircuitName), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.CircuitName.Contains(filterQuery));
                }
                if (filterOn.Equals(nameof(Circuit.State.StateName), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.State.StateName.Contains(filterQuery));
                }
                if (filterOn.Equals(nameof(Circuit.State.Zone.ZoneName), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.State.Zone.ZoneName.Contains(filterQuery));
                }
                if (filterOn.Equals(nameof(Circuit.State.Zone.TechnicalRegion.RegionName), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.State.Zone.TechnicalRegion.RegionName.Contains(filterQuery));
                }
                if (filterOn.Equals(nameof(Circuit.AccountManager), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.AccountManager.Contains(filterQuery));
                }
                if (filterOn.Equals(nameof(Circuit.ProjectManager), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.ProjectManager.Contains(filterQuery));
                }
                //TERMINATED, ACTIVE, SUSPENDED
                if (filterOn.Equals(nameof(Circuit.CircuitState.Name), StringComparison.OrdinalIgnoreCase))
                {
                    circuits = circuits.Where(q => q.CircuitState.Name.Contains(filterQuery));
                }
            }

            //Sorting
            switch (sortBy)
            {
                case "name_desc":
                    circuits = circuits.OrderByDescending(q => q.CircuitName);
                    break;
                case "name":
                    circuits = circuits.OrderBy(q => q.CircuitName);
                    break;
                case "bandwidth_desc":
                    circuits = circuits.OrderByDescending(q => q.Bandwidth);
                    break;
                case "bandwidth":
                    circuits = circuits.OrderBy(q => q.Bandwidth);
                    break;

                case "service_desc":
                    circuits = circuits.OrderByDescending(q => q.Service.Name);
                    break;
                case "service":
                    circuits = circuits.OrderBy(q => q.Service.Name);
                    break;
                case "state_desc":
                    circuits = circuits.OrderByDescending(q => q.State.StateName);
                    break;
                case "state":
                    circuits = circuits.OrderBy(q => q.State.StateName);
                    break;
                case "zone_desc":
                    circuits = circuits.OrderByDescending(q => q.State.Zone.ZoneName);
                    break;
                case "zone":
                    circuits = circuits.OrderBy(q => q.State.Zone.ZoneName);
                    break;
                case "region_desc":
                    circuits = circuits.OrderByDescending(q => q.State.Zone.TechnicalRegion.RegionName);
                    break;
                case "region":
                    circuits = circuits.OrderBy(q => q.State.Zone.TechnicalRegion.RegionName);
                    break;
                case "accountManager_desc":
                    circuits = circuits.OrderByDescending(q => q.AccountManager);
                    break;
                case "accountManager":
                    circuits = circuits.OrderBy(q => q.AccountManager);
                    break;
                case "address_desc":
                    circuits = circuits.OrderByDescending(q => q.ServiceAddress);
                    break;
                case "address":
                    circuits = circuits.OrderBy(q => q.ServiceAddress);
                    break;
                case "ippop_desc":
                    circuits = circuits.OrderByDescending(q => q.IPPoP.IPPoPName);
                    break;
                case "ippop":
                    circuits = circuits.OrderBy(q => q.IPPoP.IPPoPName);
                    break;

                //The default is ordering by name
                default:
                    circuits = circuits.OrderBy(q => q.CircuitName);
                    break;
            }


            pageNumber ??= 1;

            int count = circuits.Count();
            circuits = circuits.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            //Query only gets sent to the database from here.
            return (await circuits.AsNoTracking().ToListAsync(),count);
        }

        public async Task<CircuitCreateDropdownViewModel> GetNewCircuitDropdownValues()
        {
            var response = new CircuitCreateDropdownViewModel()
            {
                Clients = await context.Clients.
                            OrderBy(q => q.ClientName).
                            AsNoTracking().
                            ToListAsync(),

                States = await context.States.
                            OrderBy(q => q.StateName).
                            AsNoTracking().
                            ToListAsync(),

                Services = await context.Services.
                            OrderBy(q => q.Name).
                            AsNoTracking().
                            ToListAsync(),

                CircuitStates = await context.CircuitStates.
                             OrderBy(q => q.Name).
                             AsNoTracking().
                             ToListAsync(),

                IPPoPs = await context.IPPoPs.
                              OrderBy(q => q.IPPoPName).
                              AsNoTracking().
                              ToListAsync(),

                LastMileDevices = await context.LastMileDevices.
                            OrderBy(q => q.LastMileDeviceName).
                            AsNoTracking().
                            ToListAsync()
            };

            return response;
        }


    }
}
