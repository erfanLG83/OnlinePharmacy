using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Domain.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string UrlAddress { get; set; }
        public string Model { get; set; }
        public string Barcode { get; set; }
        public string StoreCode { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        // نحوه استفاده
        public string ConsumptionInstruction { get; set; }
        // نحوه نگهداری
        public string HowMaintain { get; set; }
        // توصیه ها و هشدارها , جدا شده با ;
        public string RecommendationsAndWarningsList { get; set; }
        // ویژگی ها , جدا شده با ;
        public string PropertiesList { get; set; }
        // ترکیبات در قالب 
        // نام : مقدار
        //جدا شده با ;
        public string CompoundsList{ get; set; }
        public long Price { get; set; }
        // درصد تخفیف
        public int? DiscountPercent { get; set; }
        public bool IsDeleted { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int MainCategoryId { get; set; }
        public MainCategory MainCategory { get; set; }
        public int? SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public List<ProductImage> Images { get; set; }
        public List<FavoriteProduct> FavoriteProducts { get; set; }
    }
}
