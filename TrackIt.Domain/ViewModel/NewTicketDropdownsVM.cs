
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackIt.Domain.ViewModel
{
    public class NewTicketDropdownsVM
    {
        public NewTicketDropdownsVM( Client client)
        {
            Clients = [];
            Circuits = [];
            TicketTypes = [];
            TicketStatuses = [];
            TicketPriorities = [];
        }

        public List<Client> Clients { get; set; }
        public List<Circuit> Circuits { get; set; }
        public List<TicketType> TicketTypes { get; set; }
        public List<TicketStatus> TicketStatuses { get; set; }
        public List<TicketPriority> TicketPriorities { get; set; }

    }
}
