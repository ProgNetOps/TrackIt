using Microsoft.AspNetCore.Identity;
using TrackIt.Domain.ViewModel;
using TrackIt.Domain;
using TrackIt.Repository.Services;
using TrackIt.Persistence;
using TrackIt.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace TrackIt.Repository.Implementations;

public class AdminService(RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager,
    AppDbContext context) : IAdminService
{
    private IEnumerable<ApplicationUser?>? AllUsers => userManager.Users.
        Include(x => x.UserCategory).
        AsNoTracking().ToList();

    public async Task<IEnumerable<Employee?>?> GetAllEmployees()
    {
        IEnumerable<Employee> employees = from allUsers in context.Employees.
                                          Include(q => q.UserCategory).
                                          Include(q => q.Unit).
                                          Include(q => q.LineManager).
                                          Include(q => q.State).
                                          Include(q =>q.Gender).
                                          OrderBy(q => q.FirstName).
                                          ToList()
                                          select allUsers;


        return employees;
    }

    public async Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel roleModel)
    {
        IdentityRole identityRole = new IdentityRole
        {
            Name = roleModel.RoleName
        };
        return await roleManager.CreateAsync(identityRole);
    }

    public IEnumerable<IdentityRole> GetAllRoles()
    {
        return roleManager.Roles.OrderBy(q => q.Name);
    }

    public async Task<IdentityRole?> EditRoleAsync(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is null)
        {
            return null;
        }
        else
        {
            return role;
        }
    }

    public async Task<IdentityResult?> UpdateRoleAsync(EditRoleViewModel model)
    {
        var existingRole = await roleManager.FindByIdAsync(model.Id);
        if (existingRole is null)
        {
            return null;
        }
        else
        {
            existingRole.Name = model.RoleName;
            return await roleManager.UpdateAsync(existingRole);
        }
    }

    public async Task<IEnumerable<UserRoleViewModel>?> EditUsersInRoleAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return null;
        }
        else
        {
            var model = new List<UserRoleViewModel>();
            foreach(var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                //Set values for isselected property
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected= false;
                }

                model.Add(userRoleViewModel);
            }

            return model;
        }
    }

    public async Task<(IEnumerable<ApplicationUser>,int)> GetFilteredSortedPagesAsync(string? filterOn, string? filterQuery, string sortBy, int? pageNumber, int pageSize)
    {
        IQueryable<ApplicationUser> users = from appusers in userManager.Users.
                                            Include(q => q.UserCategory)
                                            select appusers;
        
        //Filtering                        
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            
            if (filterOn.Equals(nameof(ApplicationUser.Email), StringComparison.OrdinalIgnoreCase))
            {
                users = users.Where(q => q.Email.Contains(filterQuery));
            }
            if (filterOn.Equals(nameof(ApplicationUser.PhoneNumber), StringComparison.OrdinalIgnoreCase))
            {
                users = users.Where(q => q.PhoneNumber.Contains(filterQuery));
            }
            if (filterOn.Equals(nameof(ApplicationUser.UserCategory.CategoryOfApplicationUser), StringComparison.OrdinalIgnoreCase))
            {
                users = users.Where(q => nameof(q.UserCategory.CategoryOfApplicationUser).Contains(filterQuery));
            }
        }

        //Sorting
        switch (sortBy)
        {
           
            case "email_desc":
                users = users.OrderByDescending(q => q.Email);
                break;
            case "email":
                users = users.OrderBy(q => q.Email);
                break;

            
        }

        pageNumber ??= 1;

        int count = users.Count();

        users = users.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

        //Query only gets sent to the database from here.
        return (await users.AsNoTracking().ToListAsync(), count);
    }

    public async Task<ApplicationUser?> GetUserAsync(string id)
    {
        //ApplicationUser? user = await userManager.FindByIdAsync(id);
        
        ApplicationUser? user = await userManager.Users.Include(x => x.UserCategory).
            FirstOrDefaultAsync(x => x.Id==id);
        
        return user is null ? null : user;
    }

    public async Task<IdentityResult?> UpdateUserProfile(ApplicationUser user)
    {
        var updatedUser = await userManager.UpdateAsync(user);
        return updatedUser is null ? null : updatedUser;
    }

    public IEnumerable<ApplicationUser?>? GetAllUsers()
    {
        return AllUsers;
    }
       
    public async Task<EditUserDropdownViewModel?> GetUserDropdownValues(bool isStaff)
    {
        if(isStaff is true)
        {
            var response = new EditUserDropdownViewModel()
            {
                //LineManagers = context.Employees.
                //Where(q => roleManager.Roles.
                //Select(x => x.Name).ToList().
                //Contains("Line Manager")

                //OfficeLocations = context.States.
                //OrderBy(q => q.StateName).
                //AsNoTracking().
                //ToList(),

                //UserCategories = await context.UserCategories.
                //Where(q => q.CategoryOfApplicationUser.
                //Contains("Staff")).
                //OrderBy(q => q.CategoryOfApplicationUser).
                //AsNoTracking().
                //ToListAsync(),
            };
            return response;
        }
        else
        {
            var response = new EditUserDropdownViewModel()
            {
                //LineManagers = context.Employees.
                //Where(q => q.FullName.Count() > 6).
                //OrderBy(q => q.FullName).
                //ToList(),


                OfficeLocations = context.States.
                OrderBy(q => q.StateName).
                AsNoTracking().
                ToList(),

                UserCategories = context.UserCategories.
                Where(q => q.CategoryOfApplicationUser.
                Contains("Staff") == false).
                OrderBy(q => q.CategoryOfApplicationUser).
                AsNoTracking().
                ToList()
            };
            return response;
        }
    }
             
}