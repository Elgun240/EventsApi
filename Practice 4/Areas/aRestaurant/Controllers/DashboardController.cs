using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Practice_4.Areas.aRestaurant.Controllers
{
    [Area("aRestaurant")]
    [Authorize(Roles = "Restaurant")]
    public class DashboardController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
