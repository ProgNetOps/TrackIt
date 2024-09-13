using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.ViewModel;

public class EditUserDropdownViewModel
{
    public EditUserDropdownViewModel()
    {
        OfficeLocations = [];
        LineManagers = [];
        UserCategories = [];
    }
    public IEnumerable<State>? OfficeLocations { get; set; }
    public IEnumerable<ApplicationUser>? LineManagers { get; set; }
    public IEnumerable<UserCategory> UserCategories { get; set; }
}
