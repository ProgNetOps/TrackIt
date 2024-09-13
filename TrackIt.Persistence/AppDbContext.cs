
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackIt.Domain;

namespace TrackIt.Persistence;
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //To set cascading referential integrity constraint to NO ACTION on Delete
        //instead of the default CASCADE
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        //Configurations (alternatives to data annotation on the entity models)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<Zone> Zones { get; set; } = null!;
    public DbSet<TechnicalRegion> TechnicalRegions { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
    public DbSet<LinkStatus> LinkStatuses { get; set; } = null!;
    public DbSet<CircuitState> CircuitStates { get; set; } = null!;
    public DbSet<ClientCategory> ClientCategories { get; set; } = null!;
    public DbSet<Gender> Genders { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<UserCategory> UserCategories { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<BTS> BaseStations { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<IPPoP> IPPoPs { get; set; } = null!;
    public DbSet<NetworkSwitch> NetworkSwitches { get; set; } = null!;
    public DbSet<DCNRouter> DCNRouters { get; set; } = null!;
    public DbSet<Circuit> Circuits { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<TicketType> TicketTypes { get; set; } = null!;
    public DbSet<TicketStatus> TicketStatuses { get; set; } = null!;
    public DbSet<TicketPriority> TicketPriorities { get; set; } = null!;
    public DbSet<Employee>? Employees { get; set; } = null!;
    public DbSet<TechnologyPartner> TechnologyPartners { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<LastMileDevice> LastMileDevices { get; set; } = null!;
}

