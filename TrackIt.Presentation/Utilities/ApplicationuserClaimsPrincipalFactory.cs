using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TrackIt.Domain;

namespace TrackIt.Presentation.Utilities
{
    /// <summary>
    ///This class retrieves information about the logged in user
    /// </summary>
    public class ApplicationuserClaimsPrincipalFactory:UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        public ApplicationuserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options): base(userManager, roleManager, options){
        }

        /// <summary>
        /// This method is used to retrieve the values of specific properties of the logged in user
        /// </summary>
        /// <param name="user">An instance of the Identity user</param>
        /// <returns></returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            
            identity.AddClaim(new Claim("FirstName", user.FirstName ?? string.Empty));
            identity.AddClaim(new Claim("Id", user.Id ?? string.Empty));
            identity.AddClaim(new Claim("Surname", user.Surname ?? string.Empty));
            identity.AddClaim(new Claim("FullName", user.FullName ?? string.Empty));
            identity.AddClaim(new Claim("Phone", user.PhoneNumber ?? string.Empty));
            
            return identity;
        }
    }
}
