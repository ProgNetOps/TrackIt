using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class ClientCreateViewModel
    {
        /// <summary>
        /// Name of client/customer
        /// </summary>
        [Required(ErrorMessage ="Client Name is required")]
        [StringLength(100,ErrorMessage ="Name too long")]
        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }

        [Display(Name ="Client Category")]
        [Required(ErrorMessage = "Client Category is required")]
        public Guid ClientCategoryId { get; set; }

    }
}
