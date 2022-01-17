
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using WebApp.Template.Models;

namespace WebApp.Template.Controllers
{
    //controller ismini Account olarak belirttik. Bu Identity API için default isim. Bu isimden farklı bir isim verirsek, startup da belitmemiz gerekir.
    //Yetkisiz kullanıcılar giriş denerse bu controller içine yönlendirilir.
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var appUser = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (appUser != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(appUser, password, true, false).ConfigureAwait(false);

                if (!signInResult.Succeeded)
                    return View();

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}