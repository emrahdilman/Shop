using ApplicationCore.Entities.Abstract;
using ApplicationCore.Entities.Concrete;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Areas.Admin.Models;
using Shop.Models;
using System.Diagnostics;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public HomeController(ILogger<HomeController> logger, IProductsRepository productRepo,ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _productRepo.GetFilteredList
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

            return View(product);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}