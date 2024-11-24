using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class EditEmployeeViewModel:EditUserViewModel
    {
        [Phone]
        [Display(Name = "Alternate Number")]
        public string? AlternateNumber { get; set; }

        public string? LineManager { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Onboarding Date")]
        public DateOnly? OnboardingDate { get; set; }

        [Display(Name = "Office Location")]
        [Required(ErrorMessage = "Office location is required")]
        public int? OfficeLocation { get; set; }

        [Display(Name = "Select your gender")]
        public Guid? GenderId { get; set; }
        [ForeignKey(nameof(GenderId))]
        public Gender? Gender { get; set; }

    }
}
