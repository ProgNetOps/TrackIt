using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// The priority a trouble ticket has that indicates the urgency it gets from customer care agents and technical support engineers
/// </summary>
public class TicketPriority:IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public string Priority { get; set; }


}
