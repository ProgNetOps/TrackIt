using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain
{
    /// <summary>
    /// Class that represents the current status of the service - Up, Fluctuating, Slow, Down, Degraded etc
    /// </summary>
    public class LinkStatus : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
