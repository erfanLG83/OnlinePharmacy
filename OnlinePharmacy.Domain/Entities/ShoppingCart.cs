using OnlinePharmacy.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class ShoppingCart:BaseEntity
    {
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Finished { get; set; }

        public AppUser User { get; set; }
        public Order Order { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
