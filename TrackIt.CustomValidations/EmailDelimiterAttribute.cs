using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.CustomValidations
{
    /// <summary>
    /// The delimiter separating employee's surname and first name
    /// </summary>
    /// <param name="allowedDelimiter">Delimiter character</param>
    public class EmailDelimiterAttribute(char allowedDelimiter) : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string? emailPart = value?.ToString()?.Split('@')[0];

            //Evaluates to a boolean value
            return emailPart.Contains(allowedDelimiter);            
        }

    }
}
