using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Firebase.Auth;
using Kaparasz.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaparasz.Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IFirebaseAuthProvider firebaseAuthProvider;

        public AuthController(
            IFirebaseAuthProvider firebaseAuthProvider)
        {
            this.firebaseAuthProvider = firebaseAuthProvider;
        }

        // GET: /<controller>/
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /<controller>/Login
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm]LoginViewModel login)
        {
            try
            {
                var auth = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(login.Email, login.Password);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim("Firebase-Token", auth.FirebaseToken));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));

                var claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(claimsPrincipal);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(login);
            }

        }

        // POST: /<controller>/Logout
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
