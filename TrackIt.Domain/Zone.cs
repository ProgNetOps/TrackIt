using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackIt.Domain;

/// <summary>
/// Class that represents Zonal division comprising states
/// </summary>
public class Zone
{
    #region Properties

    
    [Key]
    public int Id { get; set; }


    /// <summary>
    /// Name of the region or zone
    /// </summary>
    [Required]
    [StringLength(15)]
    [Display(Name ="Zone")]
    public string ZoneName { get; set; }


    /// <summary>
    /// List of states in the region
    /// </summary>
    public List<State>? States { get; set; }
   
    public int TechnicalRegionId { get; set; }

    [ForeignKey(nameof(TechnicalRegionId))]
    public TechnicalRegion TechnicalRegion { get; set; }
    #endregion
}
