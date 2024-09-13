using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrackIt.Domain.Contract;

namespace TrackIt.Domain;
/// <summary>
/// Represents MPLS routers
/// </summary>
public class DCNRouter:IEntityBase
{

    #region Properties

    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Base station where switch is located
    /// </summary>
    public Guid? IPPoPId { get; set; }
    [ForeignKey(nameof(IPPoPId))]
    public IPPoP? IPPoP { get; set; }

    /// <summary>
    /// Name of router
    /// </summary>
    [Required]
    [Display(Name = "Router Name")]
    [StringLength(50)]
    public string? RouterName { get; set; }

    /// <summary>
    /// Type of router
    /// </summary>
    [Required]
    [Display(Name = "Router Type")]
    [StringLength(50)]
    public string? RouterType { get; set; }

    /// <summary>
    /// IP Address of router device
    /// </summary>
    [Required]
    [Display(Name = "IP Address")]
    [StringLength(15)]
    public string? ManagementIpAddress { get; set; }



    #endregion

}
