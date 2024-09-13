using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// The categorization of trouble tickets
/// </summary>
public class TicketType:IEntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string TypeOfTicket { get; set; }
}
