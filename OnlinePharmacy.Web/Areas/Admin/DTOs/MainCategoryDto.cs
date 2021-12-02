using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class MainCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string Image { get; set; }
    }
}
