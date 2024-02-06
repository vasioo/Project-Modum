using Microsoft.AspNetCore.Mvc;

namespace Modum.Web.Controllers
{
    //[SuperAdminAuthorization]
    public class SuperAdminController : Controller
    {
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult SuperAdminControlMenu()
        {
            return View();
        }
    }
}
