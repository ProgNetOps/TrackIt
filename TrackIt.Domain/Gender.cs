using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;

public class Gender : IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
