using System.ComponentModel.DataAnnotations;

namespace TrackIt.Domain.ViewModel
{
    public class TicketCreateViewModel
    {
        [Display(Name ="Client Name")]
        [Required(ErrorMessage ="Client name is required")]
        public Guid? ClientId { get; set; }

        [Display(Name = "Service")]
        [Required(ErrorMessage = "Service name is required")]
        public Guid? CircuitId { get; set; }

        [Display(Name = "Ticket Type")]
        [Required(ErrorMessage = "Ticket type is required")]
        public Guid? TicketTypeId { get; set; }

        [Display(Name = "Ticket Status")]
        [Required(ErrorMessage = "Ticket status is required")]
        public Guid? TicketStatusId { get; set; }

        [Display(Name = "Ticket Priority")]
        [Required(ErrorMessage = "Ticket priority is required")]
        public Guid? TicketPriorityId { get; set; }

        [Display(Name = "Log Time")]
        [Required(ErrorMessage = "Log time is required")]
        public DateTime LoggedAt { get; set; } = DateTime.Now;

        //public Guid? TicketCreatorId { get; set; }
        //[ForeignKey(nameof(TicketCreatorId))]
        //public Ticketer? LoggedBy { get; set; }

        [Display(Name = "Title of Issue")]
        [Required(ErrorMessage = "Title of issue is required")]
        public string? IssueTitle { get; set; }

        [Display(Name = "Short description of issue")]
        [Required(ErrorMessage = "Short description of issue is required")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Full Description of issue")]
        [Required(ErrorMessage = "Full description of issue is required")]
        public string? FullDescription { get; set; }

        [Display(Name = "Closed At")]
        public DateTime ClosedAt { get; set; }

        [Display(Name = "Ticket Aging")]
        public TimeSpan? TicketAging => ClosedAt - LoggedAt;
    }
}
