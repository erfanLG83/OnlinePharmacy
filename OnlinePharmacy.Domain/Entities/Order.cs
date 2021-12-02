using OnlinePharmacy.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class Order
    {
        public int ShoppingCartId { get; set; }
        public string UserId { get; set; }
        public long TotalPrice { get; set; }
        public string PaymentAuthority { get; set; }
        public bool SuccessfulPayment { get; set; }
        public int UserAdressId { get; set; }
        public DateTime CreateDate { get; set; }

        public AppUser User { get; set; }
        public UserAddress UserAddress { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
