﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.WebApplication1.ViewModels.Manage
{
    public class IndexViewModel
    {
        public string Username { get; set; }
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name="Phone Number")]
        public string phoneNumber { get; set; }

        public string statusMessage { get; set; }
    }
}
