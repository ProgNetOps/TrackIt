using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class ClientDetailsViewModel
    {
        public ClientDetailsViewModel()
        {
            CircuitDetailsViewModels = [];
        }
        [Key]
        public Guid Id { get; init; }

        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }

        [Display(Name = "Client Category")]
        public Guid ClientCategoryId { get; set; }

        public List<CircuitDetailsViewModel> CircuitDetailsViewModels { get; init; }
    }
}
