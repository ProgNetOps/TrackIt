using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// The current status of the ticket; used to communicates the progress on resolutions of escalated issues to customer 
/// </summary>
public class TicketStatus:IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public string Status { get; set; }
}
