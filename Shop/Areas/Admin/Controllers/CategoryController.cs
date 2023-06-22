using ApplicationCore.Entities.Concrete;
using ApplicationCore.Entities.DTO_s.CategoryDTO;
using AutoMapper;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryController( ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View(categories);
        }

        public IActionResult CreateCategory() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                
                if (await _categoryRepo.AnyCategoryName(model.Name))
                {
                    TempData["Warning"] = "Bu kategori ismi zaten var.";
                    return View(model);
                }
                var category = _mapper.Map<Category>(model);
                await _categoryRepo.AddAsync(category);
                TempData["Success"] = "Kategori oluşturuldu!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Kategori oluşturulamadı!!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            if (id > 0)
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category != null)
                {
                    var model = _mapper.Map<UpdateCategoryDTO>(category);
                    return View(model);
                }
                TempData["Error"] = "Kategori bulunamadı!!";
                return View();
            }
            TempData["Error"] = "Hatalı işlem!!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryRepo.AnyCategoryName(model.Name))
                {
                    TempData["Warning"] = "Bu kategori adı zaten kullanılıyor!!";
                    return View(model);
                }
                var category = _mapper.Map<Category>(model);
                await _categoryRepo.UpdateAsync(category);
                TempData["Success"] = "Kategori güncellendi!!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Kategori güncellenemedi!!";
            return View(model);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id > 0)
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category != null)
                {
                    await _categoryRepo.DeleteAsync(category);
                    TempData["Success"] = "Kategori silindi!!";
                }
                else
                {
                    TempData["Success"] = "Kategori bulunamadı!!";
                }
            }
            else
            {
                TempData["Warning"] = "Hatalı işlem!!";
            }
            return RedirectToAction("Index");


        }
    }
}
