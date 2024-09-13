using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackIt.CustomValidations;
/// <summary>
/// Checks that the uploaded media type is appropriate
/// </summary>
/// <param name="extensions">A comma delimited list of allowed file extensions</param>
public class PermittedFileExtensionsAttribute(string[] extensions) : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var file = value as IFormFile;
        var fileExtension = Path.GetExtension(file?.FileName);

        return extensions.Contains(fileExtension?.ToLower());
    }

}
