using EComm.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EComm.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
            => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username != "test" || model.Password != "password")
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                    return View(model);
                }
                var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
                var principal = new ClaimsPrincipal(new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, scheme));
                await HttpContext.SignInAsync(scheme, principal);
                if (model.ReturnUrl == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return LocalRedirect(model.ReturnUrl);
            }
            return View(model);
        }
    }
}
