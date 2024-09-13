using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.Domain.Claims;

/// <summary>
/// A collection of all the claim types and values to be used to dynamically make Claims Based Access Control decisions in the application
/// </summary>
public static class ClaimsStore
{
    public static List<Claim> AllClaims = new List<Claim>()
    {
        new Claim("Create Role","Create Role"),
        new Claim("Edit Role","Edit Role"),
        new Claim("Delete Role","Delete Role"),
        new Claim("dd","Delete Role"),
    };
}
