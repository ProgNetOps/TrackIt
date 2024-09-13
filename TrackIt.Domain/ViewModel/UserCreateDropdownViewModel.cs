namespace TrackIt.Domain.ViewModel;

public class UserCreateDropdownViewModel
{
    public UserCreateDropdownViewModel()
    {
        UserCategories = [];
        Units = [];
    }
    public List<UserCategory> UserCategories { get; set; }
    public List<Unit> Units { get; set; }
    public List<Gender> Genders { get; set; }
}
