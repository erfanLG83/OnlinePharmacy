using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Data.Repositories.Contract;
using OnlinePharmacy.Data.UnitOfWork;
using OnlinePharmacy.Domain.Entities;
using OnlinePharmacy.Services.Contract;
using OnlinePharmacy.Web.Areas.Admin.DTOs;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Product> _productsRep;
        private readonly IFileWorker _fileWorker;
        public ProductsController(IUnitOfWork uow , IFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            _uow = uow;
            _productsRep = _uow.GetDynamicRepository<Product>();
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
            var products = await _uow.Context.Products
                .Include(x => x.Images)
                .Include(x => x.MainCategory)
                .Include(x => x.SubCategory)
                .Where(x =>
                !x.IsDeleted && (x.Name.Contains(searchValue) || x.EnglishName.Contains(searchValue))
            ).ToListAsync();
            switch (sortColumn)
            {
                case "price":
                    if (isAscDirection)
                        products = products.OrderBy(x => x.Price).ToList();
                    else
                        products = products.OrderByDescending(x => x.Price).ToList();
                    break;
                case "available":
                    if (isAscDirection)
                        products = products.OrderBy(x => x.Available).ToList();
                    else
                        products = products.OrderByDescending(x => x.Available).ToList();
                    break;
                case "mainCategory":
                    if (isAscDirection)
                        products = products.OrderBy(x => x.MainCategory.Name).ToList();
                    else
                        products = products.OrderByDescending(x => x.MainCategory.Name).ToList();
                    break;
                case "categoryName":
                    if (isAscDirection)
                        products = products.OrderBy(x => x.SubCategory?.Name).ToList();
                    else
                        products = products.OrderByDescending(x => x.SubCategory?.Name).ToList();
                    break;
                case "name":
                    if (isAscDirection)
                        products = products.OrderBy(x => x.Name).ToList();
                    else
                        products = products.OrderByDescending(x => x.Name).ToList();
                    break;
                case "engName":
                    if (isAscDirection)
                        products = products.OrderBy(x => x.EnglishName).ToList();
                    else
                        products = products.OrderByDescending(x => x.EnglishName).ToList();
                    break;
                default:
                    break;
            }
            var recordsTotal = products.Count();
            var data = products.Skip(skip).Take(pageSize).Select(x => new ProductDto
            {
                EngName = x.EnglishName,
                Id = x.Id,
                Image = x.Images.FirstOrDefault()?.ImageName,
                Name = x.Name,
                Available = x.Available,
                CategoryName = x.SubCategory is not null ? $"{x.SubCategory.Name } || {x.SubCategory.EnglishName}" : "---",
                MainCategoryName = $"{x.MainCategory.Name} || {x.MainCategory.EnglishName}",
                Price = x.Price.ToString("0,#")
            }).ToList();
            return Json(new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = new SelectList(
                items: _uow.Context.Brands.Select(x=>new { 
                    Value=x.Id,
                    Display =$"{x.Name} | {x.EnglishName}"
                }),
                "Value","Display"
                );
            ViewBag.MainCategories = await _uow.Context.MainCategories
                .Where(x => !x.IsDeleted)
                .Select(x => new SelectListCategoryDto
                {
                    Id = x.Id,
                    Display = $"{x.Name} | {x.EnglishName}"
                }).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    Available=productDto.Available,
                    Barcode=productDto.Barcode,
                    BrandId=productDto.BrandId,
                    Description=productDto.Description,
                    EnglishName=productDto.EnglishName,
                    Info1=productDto.Info1,
                    Info2= productDto.Info2,
                    MainCategoryId=productDto.MainCategoryId,
                    HowMaintain= productDto.HowMaintain,
                    Model= productDto.Model,
                    DiscountPercent=productDto.DiscountPercent,
                    Name=productDto.Name,
                    Price= productDto.Price,
                    UrlAddress=productDto.UrlAddress,
                    SubCategoryId=productDto.SubCategoryId,
                    StoreCode= productDto.StoreCode,
                    CompoundsList=productDto.CompoundsList,
                    RecommendationsAndWarningsList=productDto.RecommendationsAndWarningsList,
                    PropertiesList=productDto.PropertiesList,
                    ConsumptionInstruction=productDto.ConsumptionInstruction,
                    
                };
                try
                {
                    await _productsRep.CreateAsync(product);
                    await _uow.SaveChangesAsync();
                    return Redirect("/admin/products?message=created");
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/products?message=createFailed");
                }
            }
            ViewBag.Brands = new SelectList(
                items: _uow.Context.Brands.Select(x => new {
                    Value = x.Id,
                    Display = $"{x.Name} | {x.EnglishName}"
                }),
                "Value", "Display"
                );
            ViewBag.MainCategories = await _uow.Context.MainCategories
                .Where(x => !x.IsDeleted)
                .Select(x => new SelectListCategoryDto
                {
                    Id = x.Id,
                    Display = $"{x.Name} | {x.EnglishName}"
                }).ToListAsync();
            return View(productDto);
        }
        [HttpGet]
        public async Task<IActionResult> ManagementAndDetails(int id)
        {
            var product = await _productsRep.FindByIDAsync(id);
            if (product is null || product.IsDeleted)
                return NotFound();
            var expersions = new Expression<Func<Product, object>>[] { 
                x=>x.SubCategory,
                x=>x.MainCategory,
                x=>x.Brand,
            };
            product = await _productsRep.GetReferencePropertyAsync(product,expersions);
            product = await _productsRep.GetCollectionPropertyAsync(product, x => x.Images);
            return View(
                new ManagmentAndDetailsProductDto
                {
                    Available = product.Available,
                    Barcode = product.Barcode,
                    BrandName = $"{product.Brand.Name} | {product.Brand.EnglishName}",
                    EnglishName = product.EnglishName,
                    Name = product.Name,
                    CompoundsList = product.CompoundsList?.Split(';').ToList(),
                    ConsumptionInstruction = product.ConsumptionInstruction,
                    Description = product.Description,
                    DiscountPercent = product.DiscountPercent,
                    HowMaintain = product.HowMaintain,
                    ImageNames = product.Images.Select(x => x.ImageName).ToList(),
                    Id = product.Id,
                    Info1 = product.Info1,
                    Info2 = product.Info2,
                    MainCategoryName = $"{product.MainCategory.Name} | {product.MainCategory.EnglishName}",
                    Model = product.Model,
                    Price = product.Price,
                    PropertiesList = product.PropertiesList?.Split(';').ToList(),
                    RecommendationsAndWarningsList=product.RecommendationsAndWarningsList?.Split(';').ToList(),
                    StoreCode=product.StoreCode,
                    SubCategoryName=product.SubCategoryId.HasValue ? $"{product.SubCategory.Name} | {product.SubCategory.EnglishName}" : null,
                    UrlAddress=product.UrlAddress,
                    BrandImage=product.Brand.ImageName
                });
        }
        [HttpGet]
        public async Task<IActionResult> ChangeAvilableState(int id)
        {
            var product = await _productsRep.FindByIDAsync(id);
            if (product is null || product.IsDeleted)
                return NotFound();
            product.Available = !product.Available;
            try
            {
                _productsRep.Update(product);
                await _uow.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Redirect($"/admin/products/ManagementAndDetails/{product.Id}?message=changeAvilableFailed");
            }
            return Redirect($"/admin/products/ManagementAndDetails/{product.Id}?message=changeAvilableSuccess");
        }
        [HttpGet]
        public async Task<IActionResult> RemoveImage(string imageName)
        {
            if (imageName is null)
                return NotFound();
            var imgEntity = await _uow.Context.ProductImages.Where(x => x.ImageName == imageName).FirstAsync();
            if (imgEntity == null)
                return NotFound();
            try
            {
                _fileWorker.RemoveFileInPath("/images/products/"+imageName);
                _uow.Context.ProductImages.Remove(imgEntity);
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    isSuccess=false,
                    errorMessage=ex.Message
                });
            }
            return Json(new
            {
                isSuccess=true
            });

        }
        [HttpPost]
        public async Task<IActionResult> AddImage(int id,[FromForm] IFormFile image)
        {
            if (image is null)
                return BadRequest();
            var product = await _productsRep.FindByIDAsync(id);
            if (product is null || product.IsDeleted)
                return NotFound();
            try
            {
                var name = await _fileWorker.AddFileToPathAsync(image, "/images/products");
                var productImage = new ProductImage
                {
                    ImageName=name,
                    ProductId = id
                };
                await _uow.Context.ProductImages.AddAsync(productImage);
                await _uow.SaveChangesAsync();
                return Json(new
                {
                    isSuccess = true,
                    imageName = name
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    isSuccess=false,
                    message=ex.Message
                });
            }

        }
    }
}
