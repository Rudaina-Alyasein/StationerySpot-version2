using Microsoft.AspNetCore.Mvc;

namespace stationerySpot.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
