using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Models;
using System.Security.Claims;

namespace SparkServerLite.Controllers
{
    public class AccountController : Controller
    {
        public AccountController() { }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "Brandon M Bruno"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));

            //var authProperties = new AuthenticationProperties
            //{
            //    AllowRefresh = true,
            //    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
            //    IsPersistent = true,
            //};

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction(actionName: "Index", controllerName: "Admin");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
