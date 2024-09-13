using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain;
using TrackIt.Domain.Contract;
using TrackIt.Domain.ViewModel;

namespace TrackIt.Repository.Services
{
    public interface IAdminService: IFilterSortPaginate<ApplicationUser>
    {
        Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel roleModel);
        IEnumerable<IdentityRole> GetAllRoles();
        Task<IdentityRole?> EditRoleAsync(string id);
        Task<IdentityResult?> UpdateRoleAsync(EditRoleViewModel model);
        Task<IEnumerable<UserRoleViewModel>?> EditUsersInRoleAsync(string roleId);
        IEnumerable<ApplicationUser?>? GetAllUsers();
        Task<ApplicationUser?> GetUserAsync(string id);
        Task<IdentityResult?> UpdateUserProfile(ApplicationUser user);
        Task<EditUserDropdownViewModel?> GetUserDropdownValues(bool isStaff);
        Task<IEnumerable<Employee?>?> GetAllEmployees();
    }
}
