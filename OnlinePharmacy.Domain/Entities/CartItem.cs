using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class CartItem:BaseEntity
    {
        public int ProductId { get; set; }
        public int ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddDate { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }
}
