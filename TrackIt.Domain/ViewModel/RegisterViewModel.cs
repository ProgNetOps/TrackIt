
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//using System.Web.Mvc;
using TrackIt.CustomValidations;
using TrackIt.Domain.Enums;

namespace TrackIt.Domain.ViewModel;

/// <summary>
/// View model for registration of employees
/// </summary>
public class RegisterViewModel
{

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [Display(Name = "Official Email")]
    [EmailAddress]
    //Remote validation was giving weird "Email is invalid" errors
    //[Remote(action: "IsEmailInUse", controller: "Account")]
    [ValidEmailDomain("gloworld.com", ErrorMessage = "Invalid domain name")]
    [EmailDelimiter('.', ErrorMessage = "Missing email delimiter")]

    public string Email { get; set; }

    [Phone]
    [Required(ErrorMessage ="Official number is required")]
    [Display(Name ="Official phone number")]
    [RegularExpression(@"^(0805557)+[\d]{4}$",
            ErrorMessage = "Invalid Phone Format")]
    public string PhoneNumber { get; set; }


    [Required(ErrorMessage = "Create a strong password")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm your password")]
    [Display(Name = "Confirm Password")]
    [Compare(nameof(Password), ErrorMessage = "Password mismatch")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Profile Picture")]
    [PermittedFileExtensions(["jpg","jpeg","png"])]
    [MaximumFileSize(1,ErrorMessage ="Maximum file size is 1Mb")]
    public IFormFile? Photo { get; set; }

    
    [Required(ErrorMessage = "Select your gender")]
    public UserGender Gender { get; set; }
}