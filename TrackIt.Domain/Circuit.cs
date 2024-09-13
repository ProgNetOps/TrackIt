
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// Class representing a service to a customer
/// </summary>
public class Circuit : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public string? LinkID { get; set; }
    public string? ODUSerialNumber { get; set; }
    public string? IDUSerialNumber { get; set; }

    public Guid ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }

    /// <summary>
    /// Name of Client
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? CircuitName { get; set; }

    /// <summary>
    /// Address of client
    /// </summary>
    [StringLength(300)]
    public string? ServiceAddress { get; set; }

    /// <summary>
    /// Town where client's office is situated
    /// </summary>
    [StringLength(50)]
    public string? Town { get; set; }

    /// <summary>
    /// State of the federation where customer is situated
    /// </summary>
    public int StateId { get; set; }
    [ForeignKey(nameof(StateId))]
    public State State { get; set; }


    /// <summary>
    /// The latitude coordinate of the base station
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// The longitude coordinate of the base station
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Coordinates in degrees, minutes and seconds
    /// </summary>

    [StringLength(50)]
    public string? Coordinates { get; private set; }

    public Guid? ServiceId { get; set; }
    [ForeignKey(nameof(ServiceId))]
    public Service? Service { get; set; }

    public double? AnnualRevenue { get; set; }

    public double? Bandwidth { get; set; }

    public Guid? CircuitStateId { get; set; }
    [ForeignKey(nameof(CircuitStateId))]
    public CircuitState? CircuitState { get; set; }

    public Guid? IPPoPId { get; set; }
    [ForeignKey(nameof(IPPoPId))]
    public IPPoP? IPPoP { get; set; }

    [StringLength(100)]
    public string? AccountManager { get; set; }

    [StringLength(100)]
    public string? ProjectManager { get; set; }
    
    [DataType(DataType.Date)]
    public DateOnly? ServiceStartDate { get; set; }

    [StringLength(1000)]
    public string? ClientContactDetails { get; set; }

    [StringLength(1000)]
    public string? InstallersContactDetails { get; set; }

    //Technical Details
    public string? LastMileName { get; set; }
    public Guid? LastMileDeviceId { get; set; }
    [ForeignKey(nameof(LastMileDeviceId))]
    public LastMileDevice? LastMileDevice { get; set; }
    public string? TransmissionPath { get; set; }
    public double? PathLength { get; set; }
    public int? RadioManagementVLAN { get; set; }
    public int? ServiceVLAN { get; set; }
    public int? RadioFrequency { get; set; }
    public string? ManagedRadioIPAtPoP { get; set; }
    public string? ManagedRadioIPAtClient { get; set; }
    public string? ManagedRadioIPGateway { get; set; }
    public string? AssignedPublicIP { get; set; }
    public string? AssignedGateway { get; set; }
    public string? AssignedSubnetMask { get; set; }
}
