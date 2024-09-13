using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel
{
    public class UserRolesViewModel
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
