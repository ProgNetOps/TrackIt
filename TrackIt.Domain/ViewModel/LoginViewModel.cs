﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIt.CustomValidations;

namespace TrackIt.Domain.ViewModel;

public class LoginViewModel
{

    [Required,EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name ="Remember Me")]
    public bool RememberMe { get; set; }
}