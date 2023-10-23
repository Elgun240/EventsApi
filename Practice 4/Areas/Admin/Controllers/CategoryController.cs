using Microsoft.AspNetCore.Mvc;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
