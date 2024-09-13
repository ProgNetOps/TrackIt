using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain
{
    /// <summary>
    /// Class that represents the current state of the service - Active, Suspended, Terminated etc
    /// </summary>
    public class CircuitState : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
