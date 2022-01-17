using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    public class SettingsController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public SettingsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();

            if (User.Claims.FirstOrDefault(w => w.Type == Settings.DatabaseClaimType) != null)
            {
                settings.DatabaseType = (DatabaseType)int.Parse(User.Claims.First(f => f.Type == Settings.DatabaseClaimType).Value);
            }
            else
            {
                settings.DatabaseType = settings.GetDefaultDatabaseType;
            }

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDbPreference(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var newClaim = new Claim(Settings.DatabaseClaimType, databaseType.ToString());
            var claims = await _userManager.GetClaimsAsync(user);

            var databaseTypeClaim = claims.FirstOrDefault(f => f.Type == Settings.DatabaseClaimType);

            if (databaseTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, databaseTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            /// claim değişince cookie leri de güncellemek için kod içinde logout-login işlemi yapıldı.
            await _signInManager.SignOutAsync();

            var result = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, result.Properties);

            return RedirectToAction(nameof(Index));
        }
    }
}