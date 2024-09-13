using System.ComponentModel.DataAnnotations;

namespace TrackIt.Domain;

/// <summary>
/// Class that represents geographical division for technical staff's locations comprising zones
/// </summary>
public class TechnicalRegion
{
    [Key]
    public int Id { get; set; }


    /// <summary>
    /// Name of the region
    /// </summary>
    [Required]
    [StringLength(15)]
    public string RegionName { get; set; }




    /// <summary>
    /// List of states in the region
    /// </summary>
    public List<Zone>? Zones { get; set; }
}
