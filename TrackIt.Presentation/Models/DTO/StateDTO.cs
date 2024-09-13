using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain;

namespace TrackIt.Presentation.Models.DTO;

public class StateDTO
{
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
}


