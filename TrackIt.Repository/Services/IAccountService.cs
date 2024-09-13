using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;

namespace TrackIt.Repository.Services
{
    public interface IAccountService
    {
        Task<UserCreateDropdownViewModel?> GetUserDropdownValues(bool isStaff);
        Task<(ApplicationUser? user, IdentityResult? result)> CreateThirdPartyUserAsync(RegisterThirdPartyViewModel? userModel);
        Task<(ApplicationUser? user, IdentityResult? result)> CreateUserAsync(RegisterEmployeeViewModel userModel);
        Task<SignInResult> SignInAsync(LoginViewModel loginModel);
        Task SignOutAsync();
        bool IsEmailInUse(string email);
    }
}
