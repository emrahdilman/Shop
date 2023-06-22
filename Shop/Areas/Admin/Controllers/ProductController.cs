using ApplicationCore.Entities.Abstract;
using ApplicationCore.Entities.Concrete;
using ApplicationCore.Entities.DTO_s.ProductDTO;
using AutoMapper;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Areas.Admin.Models;
using System.Data;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductsRepository productRepo, IMapper mapper, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetFilteredList
                (
                    select: x => new ProductVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Image = x.Image,
                        UnitPrice = x.UnitPrice,
                        CategoryName = x.Category.Name,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate
                    },
                    where: x => x.Status != Status.Passive,
                    orderby: x => x.OrderByDescending(z => z.CreatedDate),
                    join: x => x.Include(z => z.Category)
                );

            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = new SelectList
                (
                    await _categoryRepo.GetAllAsync(), "Id", "Name"
                );
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (model.UploadImage != null)
                {
                    //Resmin yükleneceği yolu bir değişkene aktar.
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    imageName = $"{Guid.NewGuid().ToString()}_{model.UploadImage.FileName}";
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                var product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _productRepo.AddAsync(product);
                TempData["Success"] = "Ürün kaydedildi!!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Ürün kaydedilemedi!!";
            return View(model);
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            UpdateProductDTO model = new UpdateProductDTO
            {
                Categories = await _categoryRepo.GetAllAsync(),
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                Image = product.Image,
                CategoryId = product.CategoryId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (model.UploadImage != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(model.Image, "noimage.png"))
                    {
                        string oldPath = Path.Combine(uploadDir, model.Image);

                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    imageName = $"{Guid.NewGuid().ToString()}_{model.UploadImage.FileName}";

                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }

                var product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _productRepo.UpdateAsync(product);
                TempData["Success"] = "Ürün güncellendi!!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Ürün güncellenemedi!!";
            return View(model);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id > 0)
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product != null)
                {
                    await _productRepo.DeleteAsync(product);
                    TempData["Success"] = "Ürün silindi!!";
                    return RedirectToAction("Index");
                }
                TempData["Warning"] = "Ürün bulunamadı!!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Ürün silinemedi!!";
            return RedirectToAction("Index");

        }
    }
}
