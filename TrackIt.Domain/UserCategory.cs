using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// A categorization for users of the application
/// </summary>
public class UserCategory:IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string? CategoryOfApplicationUser { get; set; }
}
