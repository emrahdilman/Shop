using ApplicationCore.Entities.Concrete;
using ApplicationCore.Entities.DTO_s.AccountDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        

        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.UserName, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("LogIn");
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        

        [AllowAnonymous]
        public IActionResult LogIn() => View();

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Yanlış giriş!!");
                }
                TempData["ErrorLogIn"] = "Kullanıcı adı veya şifre yanlış!!";
            }
            return View(model);
        }

        

        public async Task<IActionResult> EditUser()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var model = new UserUpdateDTO(user);
                return View(model);
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.UserName = model.UserName;
                if (model.Password != null)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                }
                user.Email = model.Email;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Profiliniz güncellendi!!";

                }
                else
                    TempData["Error"] = "Profiliniz güncellenemedi!!";
            }
            return View(model);
        }

        

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["LogOut"] = "Çıkış yapıldı!!!";
            return RedirectToAction("LogIn");
        }

        
    }
}
