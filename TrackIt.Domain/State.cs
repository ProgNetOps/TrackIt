using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackIt.Domain;

/// <summary>
/// Class that represents the State of the Federation
/// </summary>
public class State
{
    #region Properties
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Name of the State
    /// </summary>
    [Required]
    [StringLength(20)]
    [Display(Name = "State")]
    public string StateName { get; set; }

    public int ZoneId { get; set; }

    [ForeignKey(nameof(ZoneId))]
    public Zone Zone { get; set; }

    #endregion
}
