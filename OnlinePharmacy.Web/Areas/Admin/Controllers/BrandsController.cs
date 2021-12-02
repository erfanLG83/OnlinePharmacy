using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.Data.Repositories.Contract;
using OnlinePharmacy.Data.UnitOfWork;
using OnlinePharmacy.Domain.Entities;
using OnlinePharmacy.Web.Areas.Admin.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Brand> _repository;
        public BrandsController(IUnitOfWork uow)
        {
            _uow = uow;
            _repository = _uow.GetDynamicRepository<Brand>();
        }
        public IActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            // Skiping number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var isAscDirection = sortColumnDirection == null || sortColumnDirection == "asc";
            
            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            var brands = await _repository.FindByConditionAsync(
                filter:x=>x.EnglishName.Contains(searchValue)||x.Name.Contains(searchValue)
                );
            switch (sortColumn)
            {
                case "name":
                    if (isAscDirection)
                        brands = brands.OrderBy(x => x.Name).ToList();
                    else
                        brands = brands.OrderByDescending(x => x.Name).ToList();
                    break;
                case "engName":
                    if (isAscDirection)
                        brands = brands.OrderBy(x => x.EnglishName).ToList();
                    else
                        brands = brands.OrderByDescending(x => x.EnglishName).ToList();
                    break;
                default:
                    break;
            }
            var recordsTotal = brands.Count();
            var data = brands.Skip(skip).Take(pageSize).Select(x => new BrandDto
            {
                EngName=x.EnglishName,
                Id=x.Id,
                Image=x.ImageName,
                Name=x.Name
            }).ToList();
            return Json(new
            {
                draw,
                recordsFiltered=recordsTotal,
                recordsTotal,
                data
            });
        }

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto brandDto)
        {
            if (ModelState.IsValid)
            {
                Brand brand = new Brand
                {
                    EnglishName=brandDto.EnglishName,
                    IntroductionText=brandDto.IntroducationText,
                    Name=brandDto.Name
                };
                if(brandDto.Image is not null)
                {
                }
                try
                {
                    await _repository.CreateAsync(brand);
                    await _uow.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/brands?message=createFailed");
                }
                return Redirect("/admin/brands?message=created");
            }
            return View(brandDto);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _repository.FindByIDAsync(id);
            if (brand is null)
                return NotFound();
            return View(new EditBrandDto
            {
                Id=brand.Id,
                EnglishName=brand.EnglishName,
                Name=brand.Name,
                IntroducationText=brand.IntroductionText
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditBrandDto editBrand)
        {
            var brand = await _repository.FindByIDAsync(editBrand.Id);
            if (brand is null)
                return NotFound();
            brand.EnglishName = editBrand.EnglishName;
            brand.IntroductionText = editBrand.IntroducationText;
            brand.Name = editBrand.Name;
            try
            {
                _repository.Update(brand);
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Redirect("/admin/brands?message=editFailed");
            }
            return Redirect("/admin/brands?message=edited");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _repository.FindByIDAsync(id);
            if (brand is null)
                return Json(new
                {
                    IsSuccess = false
                });
            try
            {
                _repository.Delete(brand);
                await _uow.SaveChangesAsync();
                return Json(new
                {
                    IsSuccess = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false
                });
            }
        }
    }
}
