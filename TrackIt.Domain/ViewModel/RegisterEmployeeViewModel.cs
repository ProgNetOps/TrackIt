using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel;

public class RegisterEmployeeViewModel: RegisterViewModel
{

    [Required(ErrorMessage = "Unit is required")]
    [Display(Name = "Unit")]
    public Guid? UnitId { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [Display(Name ="Gender")]
    public Guid? GenderId { get; set; }

    [Required(ErrorMessage ="Profile picture is required")]
    public IFormFile? Photo { get; set; }

}
