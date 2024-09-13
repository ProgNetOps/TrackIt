using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Presentation.Models.DTO;
public class ServiceDTO
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

}



