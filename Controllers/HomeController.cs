using magaApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using magaApp.Data;
using magaApp.ViewModels;

namespace magaApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _appDbContext;
        public HomeController(ILogger<HomeController> logger, AppDBContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Explorer()
        {
            
            List<User> users = _appDbContext.Users.ToList();

            
            List<ExplorerViewModel> viewModel = users.Select(u => new ExplorerViewModel
            {
                IdUser = u.IdUser,
                FullName = u.FullName,
                Email = u.Email
            }).ToList();

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult>GoOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
