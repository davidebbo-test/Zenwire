using System.Web.Mvc;

namespace Zenwire.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Services/
        public ActionResult Services()
        {
            return View("Services");
        }
    }
}
