using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlinePharmacy.Data.Repositories.Contract;
using OnlinePharmacy.Data.UnitOfWork;
using OnlinePharmacy.Domain.Entities;
using OnlinePharmacy.Web.Areas.Admin.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.Controllers
{
    public class SubCategoriesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<SubCategory> _categoryRepo;
        public SubCategoriesController(IUnitOfWork uow)
        {
            _uow = uow;
            _categoryRepo = _uow.GetDynamicRepository<SubCategory>();
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
            var subCategories = await _categoryRepo.FindByConditionAsync(
                filter: x => !x.IsDeleted && (x.EnglishName.Contains(searchValue)
                || x.Name.Contains(searchValue)
                || x.MainCategory.Name.Contains(searchValue)
                || (x.ParentId.HasValue && x.ParentSubCategory.Name.Contains(searchValue))),
                null, x => x.MainCategory, x => x.ParentSubCategory);
            switch (sortColumn)
            {
                case "name":
                    if (isAscDirection)
                        subCategories = subCategories.OrderBy(x => x.Name).ToList();
                    else
                        subCategories = subCategories.OrderByDescending(x => x.Name).ToList();
                    break;
                case "engName":
                    if (isAscDirection)
                        subCategories = subCategories.OrderBy(x => x.EnglishName).ToList();
                    else
                        subCategories = subCategories.OrderByDescending(x => x.EnglishName).ToList();
                    break;
                case "mainCategoryName":
                    if (isAscDirection)
                        subCategories = subCategories.OrderBy(x => x.MainCategory.Name).ToList();
                    else
                        subCategories = subCategories.OrderByDescending(x => x.EnglishName).ToList();
                    break;
                default:
                    break;
            }
            var recordsTotal = subCategories.Count();
            var data = subCategories.Skip(skip).Take(pageSize).Select(x => new SubCategoryDto
            {
                EngName = x.EnglishName,
                Id = x.Id,
                Image = x.ImageName,
                Name = x.Name,
                MainCategoryName = $"{x.MainCategory.Name} | {x.MainCategory.EnglishName}",
                ParentName = x.ParentId.HasValue ? $"{x.ParentSubCategory.Name} | {x.ParentSubCategory.EnglishName}" : "---"
            }).ToList();
            return Json(new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data
            });
        }

        public IActionResult Create()
        {
            var mainCategories = _uow.Context.MainCategories.Select(x => new SelectListCategoryDto
            {
                Id = x.Id,
                Display = $"{x.Name} | {x.EnglishName}"
            });
            ViewBag.MainCategories = new SelectList(mainCategories, "Id", "Display");
            return View();
        }
        public async Task<IActionResult> LoadSubCategories(int id, int selfId = -1)
        {
            var subCategories = await _categoryRepo.FindByConditionAsync(
                filter: x => !x.IsDeleted && x.MainCategoryId == id && !x.ParentId.HasValue && x.Id != selfId
                );
            return Json(
                subCategories.Select(x => new SelectListCategoryDto
                {
                    Id = x.Id,
                    Display = $"{x.Name} | {x.EnglishName}"
                }));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSubCategoryDto categoryDto)
        {
            ViewBag.MainCategories = _uow.Context.MainCategories.Select(x => new SelectListCategoryDto
            {
                Id = x.Id,
                Display = $"{x.Name} | {x.EnglishName}"
            });
            if (ModelState.IsValid)
            {
                var mainCategory = await _uow.Context.MainCategories.FindAsync(categoryDto.MainCategoryId);
                if (mainCategory is null)
                {
                    ModelState.AddModelError("", "دسته بندی اصلی انتخاب شده اشتباه است");
                    return View(categoryDto);
                }
                if (categoryDto.ParentId.HasValue)
                {
                    var parent = await _categoryRepo.FindByIDAsync(categoryDto.ParentId);
                    if (parent.MainCategoryId != mainCategory.Id)
                    {
                        ModelState.AddModelError("", "دسته پدر انتخاب شده به دسته اصلی نامربوط است !");
                        return View(categoryDto);
                    }
                }
                SubCategory category = new()
                {
                    EnglishName = categoryDto.EnglishName,
                    IntroductionText = categoryDto.IntroducationText,
                    IsDeleted = false,
                    Name = categoryDto.Name,
                    MainCategoryId = categoryDto.MainCategoryId,
                    ParentId = categoryDto.ParentId,
                };
                category.IntroductionImageName = "test.png";
                if (categoryDto.Image is not null)
                {
                }
                if (categoryDto.IntroducationImage is not null)
                {
                }
                try
                {
                    await _categoryRepo.CreateAsync(category);
                    await _uow.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/subcategories?message=createFailed");
                }
                return Redirect("/admin/subcategories?message=created");
            }
            return View(categoryDto);
        }
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepo.FindByIDAsync(id);
            if (category is null)
                return NotFound();
            await _uow.Context.Entry(category).Collection(x => x.ChildSubCategories).LoadAsync();
            await _uow.Context.Entry(category).Reference(x => x.ParentSubCategory).LoadAsync();
            await _uow.Context.Entry(category).Reference(x => x.MainCategory).LoadAsync();
            var categoryModel = new DetailsSubCategory
            {
                EngName = category.EnglishName,
                Id = category.Id,
                ImageName = category.ImageName,
                IntroducationImageName = category.IntroductionImageName,
                IntroducationText = category.IntroductionText,
                Name = category.Name,
                MainCategoryName = $"{category.MainCategory.Name} | {category.MainCategory.EnglishName}",
                ParentName = category.ParentId.HasValue ? $"{category.ParentSubCategory.Name} | {category.ParentSubCategory.EnglishName}" : null,
                ChildCategories = category.ChildSubCategories
                    .Select(x => new DetailsSubCategory.ChildCategoryDetail { Id = x.Id, Name = x.Name }).ToList()
            };
            return View(categoryModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepo.FindByIDAsync(id);
            if (category is null)
                return NotFound();
            ViewBag.MainCategories = _uow.Context.MainCategories
                .Where(x => !x.IsDeleted)
                .Select(x => new SelectListCategoryDto
                {
                    Id = x.Id,
                    Display = $"{x.Name} | {x.EnglishName}"
                });
            await _uow.Context.Entry(category).Collection(x => x.ChildSubCategories).LoadAsync();
            if (category.ChildSubCategories.Any())
                ViewBag.Children = category.ChildSubCategories.Select(x => $"{x.Name} | {x.EnglishName}");
            return View(new EditSubCategoryDto
            {
                Id = category.Id,
                EnglishName = category.EnglishName,
                Name = category.Name,
                IntroducationText = category.IntroductionText,
                MainCategoryId = category.MainCategoryId,
                ParentId = category.ParentId,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditSubCategoryDto categoryDto)
        {
            var category = await _categoryRepo.FindByIDAsync(categoryDto.Id);
            if (category is null)
                return NotFound();

            await _uow.Context.Entry(category).Collection(x => x.ChildSubCategories).LoadAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ChildSubCategories.Any())
                    {
                        if (categoryDto.ParentId.HasValue)
                        {
                            var childrens = category.ChildSubCategories.ToList();
                            childrens.ForEach(item =>
                            {
                                item.ParentId = null;
                            });
                            _categoryRepo.UpdateRange(childrens);
                        }
                        else if (category.MainCategoryId != categoryDto.MainCategoryId)
                        {
                            var childrens = category.ChildSubCategories.ToList();
                            childrens.ForEach(item =>
                            {
                                item.MainCategoryId = categoryDto.MainCategoryId;
                            });
                            _categoryRepo.UpdateRange(childrens);
                        }
                    }

                    category.EnglishName = categoryDto.EnglishName;
                    category.IntroductionText = categoryDto.IntroducationText;
                    category.MainCategoryId = categoryDto.MainCategoryId;
                    category.Name = categoryDto.Name;
                    category.ParentId = categoryDto.ParentId;
                    await _uow.SaveChangesAsync();
                    return Redirect("/admin/subcategories?message=edited");
                }
                catch (Exception)
                {
                    return Redirect("/admin/subcategories?message=editFailed");
                }
            }

            if (category.ChildSubCategories.Any())
                ViewBag.Children = category.ChildSubCategories.Select(x => $"{x.Name} | {x.EnglishName}");
            ViewBag.MainCategories = _uow.Context.MainCategories.Where(x => !x.IsDeleted).Select(x => new SelectListCategoryDto
            {
                Id = x.Id,
                Display = $"{x.Name} | {x.EnglishName}"
            });
            return View(new EditSubCategoryDto
            {
                Id = category.Id,
                EnglishName = category.EnglishName,
                Name = category.Name,
                IntroducationText = category.IntroductionText,
                MainCategoryId = category.MainCategoryId,
                ParentId = category.ParentId,
            });

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
                var childrens = _categoryRepo.FindAll().Where(x => x.ParentId.HasValue && x.ParentId == category.Id).ToList();
                var products = _uow.Context.Products.Where(x => x.SubCategoryId == category.Id).ToList();
                products.ForEach(e =>
                {
                    e.IsDeleted = true;
                });
                _categoryRepo.Update(category);
                if (childrens.Any())
                    _uow.Context.SubCategories.UpdateRange(childrens);
                if (products.Any())
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
