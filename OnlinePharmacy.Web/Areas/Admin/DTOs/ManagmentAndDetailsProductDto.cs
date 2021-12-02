using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class ManagmentAndDetailsProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public long Price { get; set; }
        public int? DiscountPercent { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string UrlAddress { get; set; }
        public string Model { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string StoreCode { get; set; }
        public bool Available { get; set; }
        // نحوه استفاده
        public string ConsumptionInstruction { get; set; }
        // نحوه نگهداری
        public string HowMaintain { get; set; }
        // توصیه ها و هشدارها , جدا شده با ;
        public List<string> RecommendationsAndWarningsList { get; set; }
        // ویژگی ها , جدا شده با ;
        public List<string> PropertiesList { get; set; }
        // ترکیبات در قالب 
        // نام : مقدار
        //جدا شده با ;
        public List<string> CompoundsList { get; set; }

        public string MainCategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public List<string> ImageNames { get; set; }

    }
}
