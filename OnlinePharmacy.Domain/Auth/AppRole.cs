using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlinePharmacy.Domain.Auth
{
    public class AppRole:IdentityRole
    {
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public List<AppUserRole> UserRoles { get; set; }
    }
}
