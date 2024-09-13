using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO
{
    public class ClientDTO
    {

        /// <summary>
        /// This uniquely identifies a client; required for staff in client's establishment to create trouble tickets
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of client/customer
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Client")]
        public string? ClientName { get; set; }

        public List<CircuitDTO> Circuits { get; set; }
        public Guid? ClientCategoryId { get; set; }

        [ForeignKey(nameof(ClientCategoryId))]
        public ClientCategory? ClientCategory { get; set; }
    }
}
