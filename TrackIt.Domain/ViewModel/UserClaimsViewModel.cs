using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = [];
        }

        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }

    
}
