using Microsoft.AspNetCore.Mvc;

namespace Practice_4.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RestaurantController : Controller
    {

        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Manage()
        {
            return View();
        }

    }
}
