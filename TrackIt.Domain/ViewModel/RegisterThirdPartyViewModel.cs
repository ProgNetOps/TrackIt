using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackIt.CustomValidations;

namespace TrackIt.Domain.ViewModel
{
    public class RegisterThirdPartyViewModel:RegisterViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "Name too long, shorten it")]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Surname name is required")]
        [StringLength(50, ErrorMessage = "Name too long, shorten it")]
        public string? Surname { get; set; }

        [Display(Name ="Name of company")]
        [Required(ErrorMessage ="Company name is required")]
        [StringLength(200,ErrorMessage ="Name too long, shorten it")]
        public string CompanyName { get; set; }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select your category")]
        public Guid UserCategoryId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        //Remote validation was giving weird "Email is invalid" errors
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }

        [Phone]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your phone number is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

    }
}
