using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class DetailsMainCategoryDto
    {
        public class SubCategory
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string ImageName { get; set; }
        public string IntroducationImageName { get; set; }
        public string IntroducationText { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
    
}
