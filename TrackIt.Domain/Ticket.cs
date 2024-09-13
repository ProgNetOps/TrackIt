using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// A trouble ticket
/// </summary>
public class Ticket : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ClientId { get; set; }
    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }

    public Guid? CircuitId { get; set; }
    [ForeignKey(nameof(CircuitId))]
    public Circuit? Circuit { get; set; }

    public Guid? TicketTypeId { get; set; }
    [ForeignKey(nameof(TicketTypeId))]
    public TicketType? TicketType { get; set; }

    public Guid? TicketStatusId { get; set; }
    [ForeignKey(nameof(TicketStatusId))]
    public TicketStatus? TicketStatus { get; set; }

    public Guid? TicketPriorityId { get; set; }
    [ForeignKey(nameof(TicketPriorityId))]
    public TicketPriority? TicketPriority { get; set; }

    public DateTime LoggedAt { get; set; }

    //public Guid? TicketCreatorId { get; set; }
    //[ForeignKey(nameof(TicketCreatorId))]
    //public Ticketer? LoggedBy { get; set; }

    public string? IssueTitle { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public DateTime ClosedAt { get; set; }

    public TimeSpan? TicketAging => ClosedAt - LoggedAt;
}
