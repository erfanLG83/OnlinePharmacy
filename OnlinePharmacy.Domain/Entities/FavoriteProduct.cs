using OnlinePharmacy.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class FavoriteProduct:BaseEntity
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }

        public AppUser User { get; set; }
        public Product Product { get; set; }
    }
}
