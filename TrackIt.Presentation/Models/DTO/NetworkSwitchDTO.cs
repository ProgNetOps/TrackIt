using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO
{
    public class NetworkSwitchDTO
    {
        [Key]
        public Guid Id { get; set; }



        /// <summary>
        /// Description of the switch from "Show running-config OR display current-config"
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Name of switch
        /// </summary>
        [Required]
        [StringLength(30)]
        [Display(Name ="Switch Alias")]
        public string? SwitchName { get; set; }

        /// <summary>
        /// Switch IP for telnet/SSH sessions
        /// </summary>
        [Required]
        [StringLength(30)]
        [Display(Name ="Management IP")]
        public string? ManagementIP { get; set; }

        /// <summary>
        /// Base station where switch is located
        /// </summary>
        public Guid? IPPoPId { get; set; }
        [ForeignKey(nameof(IPPoPId))]
        public IPPoP? IPPoP { get; set; }

        /// <summary>
        /// The backup configuration of the switch
        /// </summary>
        [StringLength(7000)]
        public string? BackupConfig { get; set; }

        /// <summary>
        /// A collection of all the interfaces on the switch
        /// </summary>
        

        /// <summary>
        /// Last date of switch config backup
        /// </summary>[Required]
        [Display(Name = "Last Backup")]
        public DateTime? DateOfLastBackup { get; set; }

    }
}
