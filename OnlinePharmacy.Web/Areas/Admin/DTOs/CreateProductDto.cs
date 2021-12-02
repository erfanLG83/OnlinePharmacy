using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class CreateProductDto
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(250,ErrorMessage ="لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "نام به لاتین")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string EnglishName { get; set; }
        [Display(Name = "ویژگی اول")]
        [MaxLength(50, ErrorMessage = "لطفا {0} را کمتر از 50 کرکتر وارد کنید")]
        public string Info1 { get; set; }
        [Display(Name = "ویژگی دوم")]
        [MaxLength(50, ErrorMessage = "لطفا {0} را کمتر از 50 کرکتر وارد کنید")]
        public string Info2 { get; set; }
        [Display(Name = "مسیر url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string UrlAddress { get; set; }
        [Display(Name = "مدل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "لطفا {0} را کمتر از 50 کرکتر وارد کنید")]
        public string Model { get; set; }
        [Display(Name = "بارکد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "لطفا {0} را کمتر از 50 کرکتر وارد کنید")]
        public string Barcode { get; set; }
        [Display(Name = "کد انبار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "لطفا {0} را کمتر از 50 کرکتر وارد کنید")]
        public string StoreCode { get; set; }
        public bool Available { get; set; }
        [Display(Name = "توضیحات محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        // نحوه استفاده
        public string ConsumptionInstruction { get; set; }
        // نحوه نگهداری => "... \n ..."
        public string HowMaintain { get; set; }
        // توصیه ها و هشدارها
        public string RecommendationsAndWarningsList { get; set; }
        // ویژگی ها
        public string PropertiesList { get; set; }
        // ترکیبات در قالب جیسان
        public string CompoundsList { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long Price { get; set; }
        // درصد تخفیف
        [Display(Name = "درصد تخفیف")]
        [Range(1,99,ErrorMessage ="عدد ورودی برای {0} باید در رنج 1 تا 99 باشد")]
        public int? DiscountPercent { get; set; }
        [Display(Name = "برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int BrandId { get; set; }
        [Display(Name = "دسته بندی اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int MainCategoryId { get; set; }
        [Display(Name = "زیردسته")]
        public int? SubCategoryId { get; set; }
    }
}
