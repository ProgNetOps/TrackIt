using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using TrackIt.Domain;
using TrackIt.Domain.Claims;
using TrackIt.Domain.Enums;
using TrackIt.Domain.ViewModel;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackIt.Presentation.Controllers;

//[Authorize(Roles ="Admin")]
//[Authorize(Policy = "AdminPolicy")]
public class AdministrationController(IAdminService service,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ILogger<AdministrationController> logger,
    IWebHostEnvironment webHostEnvironment) : Controller
{
    [ViewData]
    public string Title { get; set; } = string.Empty;
	[ViewData]
	public string PageHeader { get; set; } = string.Empty;
    [ViewData]
    public string ActionName { get; set; } = string.Empty;


    //The maximun number of Users returned per page
    private int pageSize = 40;


    [HttpGet]
    public async Task<IActionResult> ManageUserClaims(string userId)
    {
        Title = "Manage User Claims";
        ViewBag.userId = userId;

        var user = await service.GetUserAsync(userId);

        if (user == null)
        {
           return View("_NotFound");
        }
        else
        {
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
    {
        var user = await userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
            return View("NotFound");
        }

        var claims = await userManager.GetClaimsAsync(user);
        var result = await userManager.RemoveClaimsAsync(user, claims);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot remove user existing claims");
            return View(model);
        }

        result = await userManager.AddClaimsAsync(user,
            model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot add selected claims to user");
            return View(model);
        }

        return RedirectToAction("EditUser", new { Id = model.UserId });
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        Title = "Create Role";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel)
    {

        Title = "Create Role";

        if (ModelState.IsValid)
        {
            IdentityResult result  = await service.CreateRoleAsync(roleModel);
            if (result.Succeeded)
            {                
                return RedirectToAction(nameof(AllRoles));
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                //The validation errors are shown
                return View(roleModel);
            }
        
        }
        else
        {
            //The validation errors are shown
            return View(roleModel);
        }
    }

    [HttpGet]
    public IActionResult AllRoles()
    {
		ActionName = "CreateRole";//string passed to the partial view
		Title = "All Roles";
        PageHeader = "Role Based Access Control (RBAC)";

        var roles = service.GetAllRoles();
        return View(roles);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string id)
    {
        Title = "Edit Role";
        PageHeader = "Edit Role";

        var role = await service.EditRoleAsync(id);

        if (role == null)
        {
            TempData["Failure"] = "Role not found";
            return View("_NotFound");
        }
        else
        {
            var model = new EditRoleViewModel
            {
                Id = id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

    }

    [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleViewModel model)
    {
        Title = "Edit Role";
        PageHeader = "Edit Role";

        if (ModelState.IsValid)
        {
           var result = await service.UpdateRoleAsync(model);

            if(result is null)
            {
                TempData["Failure"] = "Role not found";
                return View("_NotFound");
            }
            else
            {
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(AllRoles));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        else
        {
            return View(model);
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
            return View("NotFound");
        }
        else
        {
            try
            {
                //throw new Exception("Test Exception");

                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("AllRoles");
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"Error deleting role {ex}");

                ViewBag.ErrorTitle = $"{role.Name} role is in use";
                ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " +
                    $"in this role. If you want to delete this role, please remove the users from " +
                    $"the role and then try to delete";
                return View("Error");
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> UserDetails(string id)
    {
        Title = "User details";
        PageHeader = "User Details";
        ActionName = "EditUser";

        var user = await service.GetUserAsync(id);

        ViewBag.EditId = Guid.Parse(user?.Id);

        if (user is null)
        {
            return View("_NotFound");
        }
        else if (user is Employee employeeUser)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Id = employeeUser.Id,
                FirstName = employeeUser.FirstName,
                Surname = employeeUser.Surname,
                OnboardingDate = employeeUser.OnboardingDate,
                ExistingPhotoPath = employeeUser.PhotoPath,
                PhoneNumber = employeeUser.PhoneNumber,
                Email = employeeUser.Email,
                UnitId = employeeUser.UnitId,
                Unit = employeeUser.Unit,
                UserCategoryId = employeeUser.UserCategoryId,
                UserCategory = employeeUser.UserCategory,
                GenderId = employeeUser.GenderId,
                Gender = employeeUser.Gender,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles
            };

            return View("EmployeeDetails", employeeDetailsViewModel);
        }
        else if (user is TechnologyPartner techPartnerUser)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var userDetailsViewModel = new UserDetailsViewModel
            {
                Id = techPartnerUser.Id,
                FirstName = techPartnerUser.FirstName,
                Surname = techPartnerUser.Surname,
                ExistingPhotoPath = techPartnerUser.PhotoPath,
                PhoneNumber = techPartnerUser.PhoneNumber,
                Email = techPartnerUser.Email,
                CompanyName = techPartnerUser.CompanyName,
                UserCategoryId = techPartnerUser.UserCategoryId,
                UserCategory = techPartnerUser.UserCategory,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles
            };

            return View(userDetailsViewModel);
        }
        else //User has to be customer
        {
            var customerUser = user as Customer;

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var userDetailsViewModel = new UserDetailsViewModel
            {
                Id = customerUser.Id,
                FirstName = customerUser.FirstName,
                Surname = customerUser.Surname,
                ExistingPhotoPath = customerUser.PhotoPath,
                PhoneNumber = customerUser.PhoneNumber,
                Email = customerUser.Email,
                UserCategoryId = customerUser.UserCategoryId,
                UserCategory = customerUser.UserCategory,
                CompanyName = customerUser.CompanyName,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles
            };
            return View(userDetailsViewModel);
        }
    }

    //Edit employee users
    [HttpGet]
    public async Task<IActionResult> EditUser(string id)
    {
        Title = "Edit User";
        PageHeader = "Edit User";

        var userDropdownData = await service.GetUserDropdownValues(true);
        ViewBag.LineManagers = new SelectList(userDropdownData?.LineManagers, "Id", "FullName");
        ViewBag.OfficeLocations = new SelectList(userDropdownData?.OfficeLocations, "Id", "StateName");
        ViewBag.UserCategories = new SelectList(userDropdownData?.UserCategories, "Id","CategoryOfApplicationUser");

        var user = await service.GetUserAsync(id);

        if(user is null or not Employee)
        {
            return View("_NotFound");
        }
        else
        {
            var employeeUser = user as Employee;

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
                       
            var model = new EditEmployeeViewModel
            {
                Id = employeeUser.Id,
                GenderId = employeeUser.GenderId,
                AlternateNumber = employeeUser.AlternateNumber,
                ExistingPhotoPath = employeeUser.PhotoPath,
                OnboardingDate = employeeUser.OnboardingDate,
                OfficeAddress = employeeUser.OfficeAddress,
                OfficeLocation = employeeUser.StateId,
                LineManager = employeeUser.LineManagerId,
                UserCategoryId = employeeUser.UserCategoryId,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(EditEmployeeViewModel model)
    {
        Title = "Edit User";
        PageHeader = "Edit User";

        var userDropdownData = await service.GetUserDropdownValues(true);
        ViewBag.LineManagers = new SelectList(userDropdownData?.LineManagers, "Id", "FullName");
        ViewBag.OfficeLocations = new SelectList(userDropdownData?.OfficeLocations, "Id", "StateName");
        ViewBag.UserCategories = new SelectList(userDropdownData?.UserCategories, "Id", "CategoryOfApplicationUser");

        var user = await service.GetUserAsync(model.Id);

        if (user is null or not Employee)
        {
            return View("_NotFound");
        }
        else //User is employee
        {
            var employeeUser = user as Employee;
            if (ModelState.IsValid)
            {
                employeeUser.Id = model.Id;
                employeeUser.GenderId = model.GenderId;
                employeeUser.PhotoPath = model.ExistingPhotoPath;
                employeeUser.AlternateNumber = model.AlternateNumber;
                employeeUser.LineManagerId = model.LineManager;
                employeeUser.OnboardingDate = model.OnboardingDate;
                employeeUser.OfficeAddress = model.OfficeAddress;
                employeeUser.UserCategoryId = model.UserCategoryId;
                employeeUser.StateId = model.OfficeLocation;
                if (model.Photo is not null)
                {
                    //User has an existing photo and the photo
                    //must be deleted from the server
                    if (model.ExistingPhotoPath is not null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    //Copy the new photo to the server
                    user.PhotoPath = ProcessUploadedFile(model);
                }

                var result = await service.UpdateUserProfile(user);

                if (result.Succeeded)
                {
                    if (User.IsInRole("Administrator"))
                    {
                        return RedirectToAction(nameof(AllUsers));
                    }
                    else
                    {
                        return RedirectToAction(nameof(UserDetails), new {model.Id});
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

             
            return View(model);
        }
    }


    //[HttpPost]Getting 405 errors with post Delete
    [HttpGet]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await service.GetUserAsync(id) as Employee;

        if (user is null or not Employee)
        {
            return View("_NotFound");
        }
        else
        {            
            //User has an existing photo and the photo
            //must be deleted from the server
            if (user.PhotoPath is not null)
            {
                string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                    "images", user.PhotoPath);
                System.IO.File.Delete(filePath);
            }
            
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("AllUsers");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("AllUsers");
            }            
        }
    }


    [HttpGet]
    public async Task<IActionResult> ManageUserRoles(string userId)
    {
        Title = "Manage User Roles";
        ViewBag.userId = userId;

        var user = await service.GetUserAsync(userId);

        PageHeader = $"Roles for {user.FirstName}";

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("_NotFound");
        }

        var model = new List<UserRolesViewModel>();

        foreach (var role in roleManager.Roles)
        {
            var userRolesViewModel = new UserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };

            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.IsSelected = true;
            }
            else
            {
                userRolesViewModel.IsSelected = false;
            }

            model.Add(userRolesViewModel);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
    {
        var user = await service.GetUserAsync(userId);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("_NotFound");
        }

        var roles = await userManager.GetRolesAsync(user);
        var result = await userManager.RemoveFromRolesAsync(user, roles);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot remove user existing roles");
            return View(model);
        }

        result = await userManager.AddToRolesAsync(user,
    model.Where(x => x.IsSelected).Select(y => y.RoleName));

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot add selected roles to user");
            return View(model);
        }

        return RedirectToAction("EditUser", new { Id = userId });
    }

    [HttpGet]
    public async Task<IActionResult> EditUsersInRole(string roleId)
    {
        Title = "Add/Remove Users from role";
        PageHeader = "Add/Remove Users";
        ViewBag.roleId = roleId;

        var usersInRole = await service.EditUsersInRoleAsync(roleId);

        if (usersInRole is null)
        {
            return View("_NotFound");
        }
        else
        {
            return View(usersInRole);
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
    {
        Title = "Add/Remove Users from role";
        PageHeader = "Add/Remove Users";
        var role = await roleManager.FindByIdAsync(roleId);

        if (role is null)
        {
            return View("_NotFound");
        }
        else
        { 
            for (int i = 0; i < model.Count; i++)
            {
                var user = await service.GetUserAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected &&
                    !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected &&
                    await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }

    //[Authorize(Roles ="Administrator, Super Admin, Staff")]
    [HttpGet]
    public async Task<IActionResult> AllUsers(string? searchBy, string? searchString, string sortBy, int? pageNumber)
    {
        Title = "All Users";
        PageHeader = "Registered Users";
        ActionName = "RegisterTP";

        //SEARCH
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { "", "Search Filter" },
            { nameof(ApplicationUser.Email), "Email" },
            { nameof(ApplicationUser.FirstName), "First Name" },
            { nameof(ApplicationUser.PhoneNumber), "Phone Number" },
            { nameof(ApplicationUser.Surname), "Surname" },
            { nameof(ApplicationUser.UserCategory.CategoryOfApplicationUser), "User Category" }
        };


        /*The values will be passed to the view to persist the data on the view after the button submits the search request*/

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        ViewData["CurrentSort"] = sortBy;

        ViewData["FirstNameSortParam"] = sortBy == "firstname" ? "firstname_desc" : "firstname";

        ViewData["SurnameSortParam"] = string.IsNullOrEmpty(sortBy) || sortBy == "surname_desc" ? "surname" : "surname_desc";

        ViewData["EmailSortParam"] = sortBy == "email" ? "email_desc" : "email";

        ViewData["StateSortParam"] = sortBy == "state" ? "state_desc" : "state";



        pageNumber ??= 1;

        var (users, count) = await service.GetFilteredSortedPagesAsync(searchBy, searchString, sortBy, pageNumber, pageSize);

        ViewBag.TotalCount = count;

        return View(new PagedList<ApplicationUser>(users.ToList(), count, pageNumber.Value, pageSize));
    }

    private string ProcessUploadedFile(EditUserViewModel model)
    {
        string? uniqueFileName = null;
        if (model.Photo is not null)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Photo.CopyTo(fileStream);
            }
        }

        return uniqueFileName;
    }
}
