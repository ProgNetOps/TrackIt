using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO;

    public class BTSDTO
    {
        #region Properties
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Site Id
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Site Id of a base station is required")]
        [StringLength(10, ErrorMessage = "Maximum character count of 10 is exceeded")]
        [Display(Name = "Base Station")]
        public string? BTSName { get; set; }

        /// <summary>
        /// Address or landmark of the location of the base station
        /// </summary>
        [StringLength(300, ErrorMessage = "Maximum character count of 300 is exceeded")]
        [Display(Name = "Address/Landmark")]
        public string? LocationAddress { get; set; }

        //public List<MPLSPoP>? DCNRouters { get; set; }

        /// <summary>
        /// The Id of the state
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public int StateId { get; set; }
        [ForeignKey(nameof(StateId))]
        public State State { get; set; }

        /// <summary>
        /// The latitude of the base station
        /// </summary>
        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// The longitude of the base station
        /// </summary>
        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }

        /// <summary>
        /// The calculated coordinates values
        /// </summary>
        [StringLength(50)]
        public string? Coordinates { get; set; }
        //public string? CalculatedCoordinates => CalculateCoord();

        /// <summary>
        /// A list of all the network switches at the base station
        /// </summary>
        //public List<NetworkSwitch>? Switches { get; set; }

        #endregion
    }


    

