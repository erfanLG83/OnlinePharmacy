using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class EditSubCategoryDto
    {
        public int Id { get; set; }
        [Display(Name = "نام")]
        [RegularExpression(@"^[\u0600-\u06FF\u0698\u067E\u0686\u06AF\u00200-9]+$", ErrorMessage = "لطفا {0} را فقط با حروف فارسی و اعداد وارد نمایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(120, ErrorMessage = "لطفا {0} را کمتر از 120 کرکتر وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "نام به انگلیسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(120, ErrorMessage = "لطفا {0} را کمتر از 120 کرکتر وارد کنید")]
        [RegularExpression(@"[\u0020A-Za-z][\u0020A-Za-z0-9]*$", ErrorMessage = "لطفا {0} را فقط با حروف لاتین و اعداد وارد نمایید")]
        public string EnglishName { get; set; }
        [Display(Name = "متن معرفی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string IntroducationText { get; set; }
        public IFormFile IntroducationImage { get; set; }
        public IFormFile Image { get; set; }
        [Display(Name = "دسته بندی اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int MainCategoryId { get; set; }
        public int? ParentId { get; set; }
    }
}
