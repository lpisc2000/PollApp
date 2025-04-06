using DataAccess;
using LeonPiscopoEPSolution.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LeonPiscopoEPSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPollRepository _repo;
        public AccountController(IPollRepository repo)
        {
            _repo = repo; // constructor injection of repository
        }

        // GET: /Account/Login - show login form
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                // If already logged in, go to polls list
                return RedirectToAction("Index", "Poll");
            }
            return View(new LoginViewModel());
        }

        // POST: /Account/Login - process login
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _repo.ValidateUser(model.Username, model.Password);
            if (user == null)
            {
                // Authentication failed
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }
            // Success: store user info in session and redirect
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Poll");
        }

        // GET: /Account/Logout - log out the current user
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
