using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;

/// <summary>
/// The Sub Departments in Enterprise Department
/// </summary>
public class Unit:IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
}
