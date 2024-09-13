using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Enums;

namespace TrackIt.Domain.ViewModel;

public class EditUserViewModel
{
    public EditUserViewModel()
    {
        Claims = [];
        Roles = [];
    }

    public string Id { get; set; }
        
    public string? ExistingPhotoPath { get; set; }

    [Display(Name ="Profile Picture")]
    public IFormFile? Photo { get; set; }//Make it nullable to make it not required
    
    [Display(Name = "Office Address")]
    public string? OfficeAddress { get; set; }

    [Display(Name = "Category")]
    public Guid UserCategoryId { get; set; }

    public List<string> Claims { get; set; }

    public IList<string> Roles { get; set; }

}
