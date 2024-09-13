using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel;

public class UserRoleViewModel
{
    public string UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public bool IsSelected { get; set; }
}