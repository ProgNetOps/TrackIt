using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class EmployeeDetailsViewModel:UserDetailsViewModel
    {
        [Display(Name = "Line Manager")]
        public string? LineManagerId { get; set; }
        [ForeignKey(nameof(LineManagerId))]
        public Employee? LineManager { get; set; }

        [Display(Name = "Gender")]
        public Guid? GenderId { get; set; }
        [ForeignKey(nameof(GenderId))]
        public Gender? Gender { get; set; }

        [Display(Name = "Unit")]
        public Guid? UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Unit? Unit { get; set; }


        [Phone]
        [Display(Name = "Alternate Number")]
        public string? AlternateNumber { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Onboarding Date")]
        public DateOnly? OnboardingDate { get; set; }

        [Display(Name = "Office Location")]
        public int? OfficeLocation { get; set; }
        [ForeignKey(nameof(OfficeLocation))]
        public State? State { get; set; }
    }
}
