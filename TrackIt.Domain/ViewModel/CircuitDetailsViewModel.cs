using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackIt.Domain.ViewModel;

public class CircuitDetailsViewModel
{
    [Key]
    public Guid Id { get; init; }

    [Display(Name = "Client Name")]
    public Guid ClientId { get; init; }
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }

    [StringLength(100, ErrorMessage = "Name too Long, shorten it")]
    [Display(Name = "Service Name")]
    public string? CircuitName { get; init; }

    [StringLength(50, ErrorMessage = "Name too Long, shorten it")]
    [Display(Name = "Link/Service ID")]
    public string? LinkID { get; set; }

    [Display(Name = "ODU Serial Number")]
    public string? ODUSerialNumber { get; set; }

    [Display(Name = "IDU Serial Number")]
    public string? IDUSerialNumber { get; set; }

    [StringLength(300, ErrorMessage = "Address too Long, shorten it")]
    [Display(Name = "Service Address")]
    public string? ServiceAddress { get; init; }

    [StringLength(50)]
    public string? Town { get; init; }

    [Display(Name = "State")]
    public int StateId { get; init; }
    [ForeignKey(nameof(StateId))]
    public State State { get; set; }

    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    [StringLength(50)]
    public string? Coordinates { get; }

    [Display(Name = "Service Type")]
    public Guid? ServiceId { get; init; }
    [ForeignKey (nameof(ServiceId))]
    public Service? Service { get; set; }

    [Display(Name = "Annual Revenue")]
    public double? AnnualRevenue { get; init; }

    [Display(Name = "Bandwidth")]
    public double? Bandwidth { get; init; }

    [Display(Name = "Circuit State")]
    public Guid? CircuitStateId { get; init; }
    [ForeignKey(nameof(CircuitStateId))]
    public CircuitState? CircuitState { get; set; }

    [Display(Name = "IP PoP")]
    public Guid? IPPoPId { get; init; }
    [ForeignKey(nameof (IPPoPId))]
    public IPPoP? IPPoP { get; set; }

    [StringLength(100, ErrorMessage = "Name too long, shorten it")]
    [Display(Name = "Account Manager")]
    public string? AccountManager { get; init; }

    [StringLength(100, ErrorMessage = "Name too long, shorten it")]
    [Display(Name = "Project Manager")]
    public string? ProjectManager { get; init; }

    [StringLength(1000)]
    [Display(Name = "Installation Engineers/Riggers")]
    public string? InstallersContactDetails { get; init; }

    [DataType(DataType.Date)]
    [Display(Name = "Service Start Date")]
    public DateOnly? ServiceStartDate { get; init; }

    [StringLength(1000)]
    [Display(Name = "Client Contact's Information")]
    public string? ClientContactDetails { get; init; }

    //Technical Details

    [Display(Name = "LastMile Name")]
    public string? LastMileName { get; init; }

    [Display(Name = "LastMile Device")]
    public Guid? LastMileDeviceId { get; init; }
    [ForeignKey(nameof(LastMileDeviceId))]
    public LastMileDevice? LastMileDevice { get; set; }

    [Display(Name = "Transmission Path")]
    public string? TransmissionPath { get; init; }

    [Display(Name = "Path Length")]
    public double? PathLength { get; init; }

    public int? RadioFrequency { get; init; }

    [Display(Name = "Radio Management VLAN")]
    public int? RadioManagementVLAN { get; init; }

    [Display(Name = "Service VLAN")]
    public int? ServiceVLAN { get; init; }

    [Display(Name = "Radio IP (Glo Side)")]
    public string? ManagedRadioIPAtPoP { get; init; }

    [Display(Name = "Radio IP (Client Side)")]
    public string? ManagedRadioIPAtClient { get; init; }

    [Display(Name = "Radio IP Gateway")]
    public string? ManagedRadioIPGateway { get; init; }

    [Display(Name = "Customer Public IP")]
    public string? AssignedPublicIP { get; init; }

    [Display(Name = "Glo IP Gateway")]
    public string? AssignedGateway { get; init; }

    [Display(Name = "IP Subnet Mask")]
    public string? AssignedSubnetMask { get; init; }

}

