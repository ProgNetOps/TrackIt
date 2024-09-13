using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO;
public class ZoneDTO
{
    #region Properties


    [Key]
    public int Id { get; set; }


    /// <summary>
    /// Name of the region or zone
    /// </summary>
    [Required]
    [StringLength(15)]
    [Display(Name = "Zone")]
    public string ZoneName { get; set; }


    /// <summary>
    /// List of states in the region
    /// </summary>
    public List<StateDTO>? States { get; set; }

    public int TechnicalRegionId { get; set; }

    [ForeignKey(nameof(TechnicalRegionId))]
    public TechnicalRegion TechnicalRegion { get; set; }
    #endregion

}



