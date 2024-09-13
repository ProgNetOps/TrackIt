using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain
{
    /// <summary>
    /// The identity class for Glo Technology Partners, it is a discriminator in the database
    /// </summary>
    public class TechnologyPartner:ApplicationUser
    {
        public string CompanyName { get; set; }
        public string? OfficeAddress { get; set; }

    }
}
