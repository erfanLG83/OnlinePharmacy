using Microsoft.AspNetCore.Identity;
using OnlinePharmacy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlinePharmacy.Domain.Auth
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        #region Navigation Properties
        public List<FavoriteProduct> FavoriteProducts { get; set; }
        public List<AppUserRole> UserRoles { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<Order> Orders { get; set; }
        //public List<ProductCard> ProductCards { get; set; }
        //public List<Order> Orders { get; set; }
        #endregion
    }
}
