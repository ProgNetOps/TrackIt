using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class BTSCreateViewModel
    {
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

        [Display(Name ="State")]
        public int StateId { get; set; }

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
    }
}
