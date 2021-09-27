using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class ProductImage:BaseEntity
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
