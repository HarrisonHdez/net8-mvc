using Microsoft.AspNetCore.Mvc;
using magaApp.Data;
using magaApp.Models;
using Microsoft.EntityFrameworkCore;
using magaApp.ViewModels;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace magaApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly AppDBContext _appDbContext;
        public AccessController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserViewModels model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ViewData["Message"] = "Passwords do not match 🙂";
                return View();
            }

            User user = new User()
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
            };

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            if (user.IdUser != 0) return RedirectToAction("Login","Access");

            ViewData["Message"] = "Cannot create user";

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModels model)
        {
            User? user_found = await _appDbContext.Users
                                .Where(u =>
                                u.Email == model.Email &&
                                u.Password == model.Password
                                ).FirstOrDefaultAsync();

            if (user_found == null)
            {
                ViewData["Message"] = "No matches found";

                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user_found.FullName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
