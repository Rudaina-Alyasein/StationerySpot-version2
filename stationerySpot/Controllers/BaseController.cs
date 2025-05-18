using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace stationerySpot.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.LibraryName = HttpContext.Session.GetString("LibraryName");
            ViewBag.LibraryLogo = HttpContext.Session.GetString("LibraryLogo") ?? "~/images/default-logo.png";

            base.OnActionExecuting(context);
        }
    }
}
