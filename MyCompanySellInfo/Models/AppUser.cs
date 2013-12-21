using Microsoft.AspNet.Identity.EntityFramework;
using MyCompanySellInfo.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCompanySellInfo.Models
{
    public class AppUserModel
    {
        
        public string Id { get; set; }
        
        [Display(Name="User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
        public RolesEnum Role { get; set; }

    }
}