using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO
{
    public class IPPoPDTO
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Site Id of the point of Preference
        /// </summary>
        [Required]
        [Display(Name = "PoP")]
        [StringLength(15)]
        public string? IPPoPName { get; set; }

        public Guid? BTSId { get; set; }
        [ForeignKey(nameof(BTSId))]
        public BTS? BTS { get; set; }

        /// <summary>
        /// List of switches at the PoP
        /// </summary>
        [Display(Name = "Switches")]
        [StringLength(35)]
        public List<NetworkSwitch>? Switches { get; set; }

        /// <summary>
        /// List of DCN routers at the PoP
        /// </summary>
        [Display(Name = "Routers")]
        [StringLength(35)]
        public List<DCNRouter>? Routers { get; set; }

        [Display(Name = "Services")]
        public List<Circuit>? Circuits { get; set; }
    }
}
