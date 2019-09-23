using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Kaparasz.Web.Controllers;
using Kaparasz.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kaparasz.Web.Controllers
{
    public class AuthController : BaseController
    {
        public class ApplicationUser
        {

        };

        private readonly IFirebaseAuthProvider firebaseAuthProvider;

        public AuthController(
            IFirebaseAuthProvider firebaseAuthProvider)
        {
            this.firebaseAuthProvider = firebaseAuthProvider;
        }

        // GET: /<controller>/
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

                // TODO: betenni a tokent, hogy használja auth.FirebaseToken

                return RedirectToAction("Home", "Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(login);
            }

        }

        /*
        private FirebaseAuthProvider _authProvider;
        private SignInManager<ApplicationUser> _signInManager;

        public AuthController()
        {
            this._authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAcyaI48Vo7P3r8thKttsdUYl_bsfpG-g4"));
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            _authProvider.si
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        */
    }
}
