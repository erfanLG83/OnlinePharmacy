using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Auth
{
    public class UserAddress:BaseEntity
    {
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string ProvinceName { get; set; }
        public bool DefaultAddress { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
