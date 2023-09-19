using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.SSO;
using System.Runtime;
using System.Security.Claims;

namespace SparkServerLite.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppSettings _settings;

        public AccountController(IAppSettings appSettings)
        {
            _settings = appSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            // Override SSO login when debug mode is on
            if (_settings.Debug)
            {
                var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "000"));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, "user@test.com"));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "Admin"));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction(actionName: "Index", controllerName: "Admin");
            }

            return Redirect(url: _settings.SSOLoginURL + _settings.SSOSiteID);
        }

        public async Task<IActionResult> Authenticate(string token)
        {
            TokenStatus status = TokenService.ValidateToken(token, _settings.SSOSigningKey);

            if (status != TokenStatus.Valid)
            {
                if (status == TokenStatus.Expired)
                    return Redirect(url: _settings.SSOLoginURL + _settings.SSOSiteID);

                TempData["Error"] = $"Token not valid. Status: {status.ToString()}";
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            TokenPayload payload = TokenService.GetPayload(token);

            var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, payload.uid.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, payload.eml));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, payload.fname));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));

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
