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
    public class MainCategoriesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<MainCategory> _categoryRepo;
        public MainCategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
            _categoryRepo = _uow.GetDynamicRepository<MainCategory>();
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
            var mainCategories = await _categoryRepo.FindByConditionAsync(
                filter: x => !x.IsDeleted && ( x.EnglishName.Contains(searchValue) || x.Name.Contains(searchValue) )
                );
            switch (sortColumn)
            {
                case "name":
                    if (isAscDirection)
                        mainCategories = mainCategories.OrderBy(x => x.Name).ToList();
                    else
                        mainCategories = mainCategories.OrderByDescending(x => x.Name).ToList();
                    break;
                case "engName":
                    if (isAscDirection)
                        mainCategories = mainCategories.OrderBy(x => x.EnglishName).ToList();
                    else
                        mainCategories = mainCategories.OrderByDescending(x => x.EnglishName).ToList();
                    break;
                default:
                    break;
            }
            var recordsTotal = mainCategories.Count();
            var data = mainCategories.Skip(skip).Take(pageSize).Select(x => new MainCategoryDto
            {
                EngName = x.EnglishName,
                Id = x.Id,
                Image = x.ImageName,
                Name = x.Name
            }).ToList();
            return Json(new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data
            });
        }

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateMainCategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                MainCategory category = new()
                {
                    EnglishName=categoryDto.EnglishName,
                    IntroductionText=categoryDto.IntroducationText,
                    IsDeleted=false,
                    Name=categoryDto.Name
                };
                category.IntroductionImageName = "test.png";
                if (categoryDto.Image is not null)
                {
                }
                if(categoryDto.IntroducationImage is not null)
                {
                }
                try
                {
                    await _categoryRepo.CreateAsync(category);
                    await _uow.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/mainCategories?message=createFailed");
                }
                return Redirect("/admin/mainCategories?message=created");
            }
            return View(categoryDto);
        }
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepo.FindByIDAsync(id);
            if (category is null)
                return NotFound();
            await _uow.Context.Entry(category).Collection(x => x.SubCategories).LoadAsync();
            var categoryModel = new DetailsMainCategoryDto
            {
                EngName = category.EnglishName,
                Name = category.Name,
                IntroducationText = category.IntroductionText,
                IntroducationImageName = category.IntroductionImageName,
                ImageName = category.ImageName,
                Id = category.Id,
                SubCategories = category.SubCategories.Select(x => new DetailsMainCategoryDto.SubCategory
                {
                    Id=x.Id,
                    Name=x.Name
                }).ToList()
            };
            return View(categoryModel);
            

        }
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepo.FindByIDAsync(id);
            if (category is null)
                return NotFound();
            return View(new EditMainCategoryDto
            {
                Id = category.Id,
                EnglishName = category.EnglishName,
                Name = category.Name,
                IntroducationText = category.IntroductionText
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditMainCategoryDto editCategory)
        {
            var category = await _categoryRepo.FindByIDAsync(editCategory.Id);
            if (category is null)
                return NotFound();
            category.EnglishName = editCategory.EnglishName;
            category.IntroductionText = editCategory.IntroducationText;
            category.Name = editCategory.Name;
            try
            {
                _categoryRepo.Update(category);
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Redirect("/admin/maincategories?message=editFailed");
            }
            return Redirect("/admin/maincategories?message=edited");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.FindByIDAsync(id);
            if (category is null)
                return Json(new
                {
                    IsSuccess = false
                });
            try
            {
                category.IsDeleted = true;
                var subcategories = _uow.Context.SubCategories.Where(x => x.MainCategoryId == category.Id).ToList();
                subcategories.ForEach(e =>
                {
                    e.IsDeleted = true;
                });
                var products = _uow.Context.Products.Where(x => x.MainCategoryId == category.Id).ToList();
                products.ForEach(e =>
                {
                    e.IsDeleted = true;
                });
                _categoryRepo.Update(category);
                if(subcategories.Any())
                    _uow.Context.SubCategories.UpdateRange(subcategories);
                if(products.Any())
                    _uow.Context.Products.UpdateRange(products);
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
