using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.CustomValidations;
/// <summary>
/// Checks that the file size does not exceed the stated max value
/// </summary>
/// <param name="maxFileSize">The allowed maximum file size in Megabytes</param>
public class MaximumFileSizeAttribute(int maxFileSize) : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var file = value as IFormFile;
        byte maxFileSizeInBytes = Convert.ToByte(maxFileSize);

        return file is not null && file.Length <= maxFileSizeInBytes;        
    }
}
