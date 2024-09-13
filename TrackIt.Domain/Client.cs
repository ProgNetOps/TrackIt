
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain
{
    /// <summary>
    /// Class that represents the organization that is a corporate client/customer to Globacom Enterprise Business
    /// </summary>
    public class Client:IEntityBase
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
        [Display(Name ="Client")]
        public string? ClientName { get; set; }
        public Guid ClientCategoryId { get; set; }

        [ForeignKey(nameof(ClientCategoryId))]
        public ClientCategory ClientCategory { get; set; }

        //List of services for a particular client
        public List<Circuit>? Circuits { get; set; }


    }
}
