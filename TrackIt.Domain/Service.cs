using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;

/// <summary>
/// Types of services offered eg FTTH, internet, PRI, Leased Line etc
/// </summary>
public class Service : IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
}


