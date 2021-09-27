using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageName { get; set; }
        public string IntroductionImageName { get; set; }
        public string IntroductionText { get; set; }

        public int MainCategoryId { get; set; }
        public int? ParentId { get; set; }
        public MainCategory MainCategory { get; set; }
        public SubCategory ParentSubCategory { get; set; }
        public List<SubCategory> ChildSubCategories { get; set; }
        public List<Product> Products { get; set; }
    }
}
