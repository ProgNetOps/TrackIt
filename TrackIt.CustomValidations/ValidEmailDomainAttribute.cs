using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.CustomValidations;

/// <summary>
/// Checks that the email domain is company's
/// </summary>
/// <param name="allowedDomain">company's email domain</param>
public class ValidEmailDomainAttribute(string allowedDomain) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        string? domain = value?.ToString()?.Split('@')[1];

        //Evaluates to a boolean value
        return domain.Equals(allowedDomain, StringComparison.OrdinalIgnoreCase);
        
    }

}


