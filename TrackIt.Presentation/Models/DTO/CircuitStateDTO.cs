using System.ComponentModel.DataAnnotations;

namespace TrackIt.Presentation.Models.DTO;
public class CircuitStateDTO
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}



