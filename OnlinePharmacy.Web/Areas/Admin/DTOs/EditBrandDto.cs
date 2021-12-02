using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.DTOs
{
    public class EditBrandDto:CreateBrandDto
    {
        public int Id { get; set; }
    }
}
