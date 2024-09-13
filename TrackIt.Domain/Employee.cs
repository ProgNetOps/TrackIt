using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackIt.Domain.Enums;

namespace TrackIt.Domain;
/// <summary>
/// The identity class for Glo staff, it is a discriminator in the database
/// </summary>
public class Employee:ApplicationUser
{
    public Guid? UnitId { get; set; }
    [ForeignKey(nameof(UnitId))]
    public Unit? Unit { get; set; }
    public string? LineManagerId { get; set; }
    [ForeignKey(nameof(LineManagerId))]
    public Employee? LineManager { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Onboarding Date")]
    public DateOnly? OnboardingDate { get; set; }

    public int? StateId { get; set; }
    [ForeignKey(nameof(StateId))]
    public State? State { get; set; }

    public string? OfficeAddress { get; set; }

    [Phone]
    [Display(Name = "Alternate Number")]
    public string? AlternateNumber { get; set; }

    public Guid? GenderId { get; set; }
    [ForeignKey(nameof(GenderId))]
    public Gender? Gender { get; set; }
}
