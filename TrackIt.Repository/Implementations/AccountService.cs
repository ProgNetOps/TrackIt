using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackIt.Domain;
using TrackIt.Domain.Enums;
using TrackIt.Domain.ViewModel;
using TrackIt.Persistence;
using TrackIt.Repository.Services;

namespace TrackIt.Repository.Implementations;

public class AccountService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IWebHostEnvironment hostingEnvironment,
    AppDbContext context) :IAccountService
{
        
    public async Task<UserCreateDropdownViewModel?> GetUserDropdownValues(bool isStaff)
    {
        
        if (isStaff is true) {
            var response = new UserCreateDropdownViewModel()
            {
                //categories that employee can be in
                UserCategories = await context.UserCategories.
                Where(q => q.CategoryOfApplicationUser.
                Contains("Staff")).
                OrderBy(q => q.CategoryOfApplicationUser).
                AsNoTracking().
                ToListAsync(),

                Units = await context.Units.
                AsNoTracking().
                ToListAsync(),

                Genders = await context.Genders.
                AsNoTracking().
                ToListAsync()
            };
            return response;
        }
        else
        {
            var response = new UserCreateDropdownViewModel()
            {
                //categories that non-employees can be in
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

    public async Task<(ApplicationUser? user, IdentityResult? result)> CreateUserAsync(RegisterEmployeeViewModel? userModel)
    {
        string? uniqueFileName = ProcessUploadedFile(userModel);

        Employee appUser = new Employee()
        {
            Email = userModel.Email,
            UserName = userModel.Email,
            UnitId = userModel.UnitId,
            PhoneNumber = userModel.PhoneNumber,
            UserCategoryId = Guid.Parse("58B3483A-E72D-4768-8591-B5DA96F42896"),
            PhotoPath = uniqueFileName,
            GenderId = userModel.GenderId
        };

        //Set the first name and surname from the email value
        appUser.FirstName = appUser.Email.Split('@')[0].Split('.')[0];
        appUser.Surname = appUser.Email.Split('@')[0].Split('.')[1];

        var result = await userManager.CreateAsync(appUser, userModel.Password);

        return (appUser, result);

    }

    //To be refactored later
    public async Task<(ApplicationUser? user, IdentityResult? result)> CreateThirdPartyUserAsync(RegisterThirdPartyViewModel? userModel)
    {
        string? uniqueFileName = ProcessUploadedFile(userModel);

        
        if(userModel.UserCategoryId.ToString().Equals("C4AED73D-E5BA-466C-A0A9-C6F625752884", StringComparison.OrdinalIgnoreCase))
        {
            Customer appUser = new Customer()
            {
                Email = userModel.Email,
                UserName = userModel.Email,
                FirstName = userModel.FirstName,
                Surname = userModel.Surname,
                PhoneNumber= userModel.PhoneNumber,
                CompanyName = userModel.CompanyName,
                UserCategoryId = userModel.UserCategoryId,
                PhotoPath = uniqueFileName
            };
            var result = await userManager.CreateAsync(appUser, userModel.Password);

            return (appUser, result);
        }
        else
        {
            TechnologyPartner appUser = new TechnologyPartner()
            {
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                Surname = userModel.Surname,
                UserName = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                CompanyName = userModel.CompanyName,
                UserCategoryId = userModel.UserCategoryId,
                PhotoPath = uniqueFileName
            };
            var result = await userManager.CreateAsync(appUser, userModel.Password);

            return (appUser, result);
        }
    }

    public async Task<SignInResult> SignInAsync(LoginViewModel loginModel)
    {

        var result = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
        return result;
    }

    public async Task SignOutAsync()
    {
        await signInManager.SignOutAsync();
    }
            
    public bool IsEmailInUse(string email)
    {
        var user = userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private string? ProcessUploadedFile(RegisterViewModel model)
    {
        string? uniqueFileName = null;
        if (model.Photo is not null)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
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
