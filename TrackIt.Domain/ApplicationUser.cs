using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackIt.Domain.Enums;

namespace TrackIt.Domain;
/// <summary>
/// An abstract child class of framework's IdentityUser class, acting as an extension point for concrete Customer and Employee classes
/// </summary>
public abstract class ApplicationUser:IdentityUser
{
   
    public Guid UserCategoryId { get; set; }
    [ForeignKey(nameof(UserCategoryId))]
    public UserCategory UserCategory { get; set; }
    public string? PhotoPath { get; set; }

  

}