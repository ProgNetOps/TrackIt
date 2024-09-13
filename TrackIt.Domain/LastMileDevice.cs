using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// The transmission device for service delivery from the ip pop to customer location
/// </summary>

public class LastMileDevice:IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string? LastMileDeviceName { get; set; }
}
