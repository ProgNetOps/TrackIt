using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TrackIt.Domain.Enums
{
    //This enum values will be used for access control decisions
    public enum CategoryOfUser
    {
        [Display(Name = "Glo Client")]
        GloClient = 1,

        [Display(Name = "Glo Staff")]
        GloStaff = 2,

        [Display(Name = "Technology Partner")]
        GloTechPartner = 3,

        [Display(Name = "Ex Glo Staff")]
        ExGloStaff = 4
    }
}
