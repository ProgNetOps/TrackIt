using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO
{
    public class CircuitDTO
    {

        [Key]
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public ClientDTO Client { get; set; }

        /// <summary>
        /// Name of Client
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Circuit Name")]
        public string CircuitName { get; set; }

        /// <summary>
        /// Address of client
        /// </summary>
        [StringLength(300)]
        [Display(Name ="Service Address")]
        public string? ServiceAddress { get; set; }

        /// <summary>
        /// Town where client's office is situated
        /// </summary>
        [StringLength(50)]
        public string? Town { get; set; }

        /// <summary>
        /// State of the federation where customer is situated
        /// </summary>
        public int StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public StateDTO State { get; set; }

        /// <summary>
        /// The latitude coordinate of the base station
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// The longitude coordinate of the base station
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Coordinates in degrees, minutes and seconds
        /// </summary>

        [StringLength(50)]
        public string? Coordinates { get; private set; }

        public Guid? ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        public ServiceDTO? Service { get; set; }

        public double? AnnualRevenue { get; set; }

        public double? Bandwidth { get; set; }

        public Guid? CircuitStateId { get; set; }
        [ForeignKey("CircuitStateId")]
        public CircuitStateDTO? CircuitState { get; set; }

        [StringLength(100)]
        [Display(Name ="Account Manager")]
        public string? AccountManager { get; set; }

        [StringLength(100)]
        [Display(Name = "Project Manager")]
        public string? ProjectManager { get; set; }

        [StringLength(100)]
        public string? TAM { get; set; }
        
        public Guid? IPPoPId { get; set; }
        [ForeignKey(nameof(IPPoPId))]
        public IPPoP? IPPoP { get; set; }

        //[StringLength(50)]
        //public string? LastMileMedium { get; set; }

        //[StringLength(50)]
        //public string? LastMileEquipmentType { get; set; }

        //[StringLength(300)]
        //public string? LastMileEquipmentSpecs { get; set; }

        //[StringLength(200)]
        //public string? TransmissionPath { get; set; }

        //public double? PathLength { get; set; }

        //[StringLength(50)]
        //public string? RadioManagementVLAN
        //{
        //    get; set;
        //}

        //[StringLength(50)]
        //public string? Gateway { get; set; }

        //[StringLength(50)]
        //public string? PopRadioIP { get; set; }

        //[StringLength(50)]
        //public string? ClientRadioIP { get; set; }


        public DateTime? ServiceStartDate { get; set; }
        [StringLength(1000)]
        [Display(Name = "Client Contact Details")]
        public string? ClientContactDetails { get; set; }
    }
}
