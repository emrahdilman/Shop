using ApplicationCore.Entities.Concrete;
using ApplicationCore.Entities.DTO_s.RoleDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult CreateRole() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleDTO model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole(model.Name);
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Rol kaydedildi!!";
                    return RedirectToAction("Index");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    TempData["Error"] = error.Description;
                }
            }
            TempData["Error"] = "Bişeyler ters gitti!!";
            return View(model);
        }

        public async Task<IActionResult> AssignedUser(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<AppUser> hasRole = new List<AppUser>();
            List<AppUser> hasNotRole = new List<AppUser>();

            foreach (AppUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? hasRole : hasNotRole;
                list.Add(user);
            }

            AssignedRoleDTO model = new AssignedRoleDTO
            {
                Role = role,
                HasRole = hasRole,
                HasNotRole = hasNotRole,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignedUser(AssignedRoleDTO model)
        {
            IdentityResult result = new IdentityResult();

            foreach (var userId in model.AddIds ?? new string[] { })
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            foreach (var userId in model.DeleteIds ?? new string[] { })
            {
                AppUser user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            }

            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcılar role başarılı bir şekilde atandı!!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> RemoveRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = "Role silindi!!!";
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                TempData["Error"] = error.Description;
            }

            return RedirectToAction("Index");
        }
    }
}
