using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TrackIt.Domain;
using TrackIt.Domain.ViewModel;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.Controllers;

public class AccountController(IAccountService service, 
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager, ILogger<String> logger) : Controller
{
    [ViewData]
    public string Title { get; set; } = string.Empty;
    [ViewData]
    public string PageHeader { get; set; } = string.Empty;


    //To register Employees
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register()
    {
        Title = "Register";
        var UserDropdownData = await service.GetUserDropdownValues(true);
        ViewBag.Units = new SelectList(UserDropdownData.Units, "Id", "Name");
        ViewBag.Genders = new SelectList(UserDropdownData.Genders, "Id", "Name");

        return View();
    }


    //To register Employees
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterEmployeeViewModel userModel)
    {
        Title = "Register";
        var UserDropdownData = await service.GetUserDropdownValues(true);
        ViewBag.Units = new SelectList(UserDropdownData.Units, "Id", "Name");
        ViewBag.Genders = new SelectList(UserDropdownData.Genders, "Id", "Name");

        if (ModelState.IsValid)
        {
            //Deconstruct the valuetuple return value
            var (user, result) = await service.CreateUserAsync(userModel);

            if (result?.Succeeded is false)
            {
                foreach (var errorMessage in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errorMessage.Description);
                }
                return View(userModel);
            }
            else
            {
                Employee empUser = user as Employee;
                await signInManager.SignInAsync(empUser, isPersistent: false);

                //Define some claim types and their values
                new Claim(ClaimTypes.Email, empUser.Email);
                new Claim(ClaimTypes.Surname, empUser.Surname);
                new Claim("Phone Number", empUser.PhoneNumber);

                return RedirectToAction("EditUser", "Administration", new { id = user.Id });
            }
        }
        else
        {
            
            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();
            return View(userModel);
        }
    }


    //To register third party users
    [HttpGet]
    public async Task<IActionResult> RegisterTP()
    {
        Title = "Register";
        var UserDropdownData = await service.GetUserDropdownValues(false);
        ViewBag.Categories = new SelectList(UserDropdownData.UserCategories, "Id", "CategoryOfApplicationUser");
        
        return View();
    }


    //To register third party users
    [HttpPost]
    public async Task<IActionResult> RegisterTP(RegisterThirdPartyViewModel userModel)
    {
        Title = "Register";
        var UserDropdownData = await service.GetUserDropdownValues(false);
        ViewBag.Categories = new SelectList(UserDropdownData.UserCategories, "Id", "CategoryOfApplicationUser");

        if (ModelState.IsValid)
        {
            //Deconstruct the valuetuple return value
            var (user, result) = await service.CreateThirdPartyUserAsync(userModel);

            if (result?.Succeeded is false)
            {
                foreach (var errorMessage in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errorMessage.Description);
                }
                return View(userModel);
            }
            else
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("AllUsers", "Administration");
            }

        }
        else
        {
            return View(userModel);
        }
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        Title = "Login";
        return View();
    }


    [HttpPost]
    [AllowAnonymous]
    //Make returnUrl nullable to prevent "returnUrl is required error"
    public async Task<IActionResult> Login(LoginViewModel loginModel, string? returnUrl)
    {
        Title = "Login";

        if (ModelState.IsValid)
        {
            var result = await service.SignInAsync(loginModel);
            if (result.Succeeded)
            {
                if(string.IsNullOrEmpty(returnUrl) is false)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    //return RedirectToAction("Welcome", "Home");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(loginModel);
            }
        }
        else
        {
            return View(loginModel);
        }
    }


    public async Task<IActionResult> Logout()
    {

        await service.SignOutAsync();
        return RedirectToAction(nameof(Index), "Home");
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        Title = "Access Denied";
        PageHeader = "Access Denied";

        return View();
    }


    [AcceptVerbs("Get", "Post")]
    [AllowAnonymous]
    public async Task<IActionResult> IsEmailInUse(string email)
    {
        var result = service.IsEmailInUse(email);

        return result ? Json($"{email} is already in use") : Json(result);
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null && await userManager.IsEmailConfirmedAsync(user))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                logger.Log(LogLevel.Warning, passwordResetLink);

                return View("ForgotPasswordConfirmation");
            }
            else
            {
                return View("ForgotPasswordConfirmation");
            }
        }
        else
        {
            return View(model);
        }
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string? token, string? email)
    {
        if(token is null || email is null)
        {
            ModelState.AddModelError("", "invalid password reset token");
        }
        return View();
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if(user is not null)
            {
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("ResetPasswordConfirmation");
        }
        else
        {
            return View(model);
        }
    }

}
