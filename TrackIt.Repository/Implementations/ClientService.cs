using Microsoft.EntityFrameworkCore;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Persistence;
using TrackIt.Repository.Base;
using TrackIt.Repository.Services;

namespace TrackIt.Repository.Implementations;

public class ClientService(AppDbContext context) : SQLBaseRepository<Client>(context), IClientService
{
    public DbSet<Client> allClients => context.Clients;

   
    public async Task<Client?> GetClientDetails(Guid id)
    {
        var client = await allClients.Include(q => q.ClientCategoryId).
            AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);

        return client;
    }

    public async Task<(Client client, Guid id)> AddClientAsync(Client entity)
    {
        await allClients.AddAsync(entity);
        var newId = entity.Id;
        await context.SaveChangesAsync();

        return (entity, newId);
    }

    public override async Task<Client?> GetByIdAsync(Guid id)
    {
        var clientDetails = await allClients.Include(q => q.Circuits).
            ThenInclude(q => q.State).
            ThenInclude(q => q.Zone).
            Include(q => q.Circuits).
            ThenInclude(q => q.Service).
            FirstOrDefaultAsync(q => q.Id == id);
        return clientDetails;
    }

    public async Task<(IEnumerable<Client>,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
    {
        IQueryable<Client> clients = from customers in context.Clients.
                                     Include(q => q.ClientCategory).
                                     Include(q => q.Circuits).
                                     OrderBy(q => q.ClientName)
                                     select customers;

        //Filtering                        
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals(nameof(Client.ClientName), StringComparison.OrdinalIgnoreCase))
            {
                clients = clients.Where(q => q.ClientName.Contains(filterQuery));
            }
            if (filterOn.Equals(nameof(Client.ClientCategory), StringComparison.OrdinalIgnoreCase))
            {
                clients = clients.Where(q => q.ClientCategory.Name.Contains(filterQuery));
            }

        }


        //Sorting
        switch (sortBy)
        {
            case "name_desc":
                clients = clients.OrderByDescending(q => q.ClientName);
            break;

            case "name":
                clients = clients.OrderBy(q => q.ClientName);
            break;

            case "clientCategory_desc":
                clients = clients.OrderByDescending(q => q.ClientCategory.Name);
            break;

            case "clientCategory":
                clients = clients.OrderBy(q => q.ClientCategory.Name);
            break;

            //The default is ordering by name
            default:
                clients = clients.OrderBy(q => q.ClientName);
            break;
        }


		pageNumber ??= 1;

		int count = clients.Count();

		clients = clients.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

        //Query only gets sent to the database from here.
        return (await clients.AsNoTracking().ToListAsync(), count);
    }

    


    public async Task<ClientCreateDropdownViewModel> GetNewClientDropdownValues()
    {
        var response = new ClientCreateDropdownViewModel()
        {
            ClientCategories = await context.ClientCategories.
            OrderBy(q => q.Name).
            AsNoTracking().
            ToListAsync()
        };

        return response;
    }
}

