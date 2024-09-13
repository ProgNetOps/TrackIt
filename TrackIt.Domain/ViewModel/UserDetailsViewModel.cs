using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TrackIt.Domain.Enums;
using TrackIt.Domain;

namespace TrackIt.Domain.ViewModel;

public class UserDetailsViewModel
{
    public UserDetailsViewModel()
    {
        Claims = [];
        Roles = [];
    }
    public string Id { get; set; }

    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? ExistingPhotoPath { get; set; }

    [Display(Name = "Profile Picture")]
    public IFormFile? Photo { get; set; }

    [Phone]
    [Display(Name = "Official Number")]
    [RegularExpression(@"^(0805557)[\d]{4}$", ErrorMessage = "Invalid staff number")]
    public string? PhoneNumber { get; set; }


    [EmailAddress]
    [Display(Name = "Official Email")]
    public string Email { get; set; }


    [Display(Name = "Category")]
    public Guid? UserCategoryId { get; set; }
    [ForeignKey(nameof(UserCategoryId))]
    public UserCategory? UserCategory { get; set; }

    [Display(Name = "Office Address")]
    public string? OfficeAddress { get; set; }

    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    public List<string> Claims { get; set; }

    public IList<string> Roles { get; set; }

}
