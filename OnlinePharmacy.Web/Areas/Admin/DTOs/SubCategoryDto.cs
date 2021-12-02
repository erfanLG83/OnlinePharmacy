using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string Image { get; set; }
        public string ParentName { get; set; }
        public string MainCategoryName { get; set; }
    }
}
