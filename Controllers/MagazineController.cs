using Microsoft.AspNetCore.Mvc;

namespace magaApp.Controllers
{
    public class MagazineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
 