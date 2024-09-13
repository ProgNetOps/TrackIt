using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// The industry/category into which Glo Enterprise customers fall eg Banking, Oil and Gas, SME etc
/// </summary>
public class ClientCategory : IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Client>? Clients { get; set; }
}
