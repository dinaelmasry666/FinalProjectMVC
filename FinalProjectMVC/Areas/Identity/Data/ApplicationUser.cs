﻿using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace FinalProjectMVC.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        /*
         Anything added to this class will be migrated into the 
         `Asp.netUsers table in the database. 
        */
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProfilePicture { get; set; }

        public virtual List<Address>? Addresses { get; set; }

        [DefaultValue(false)]
        public bool IsBlocked { get; set; } = false;
    }
}
