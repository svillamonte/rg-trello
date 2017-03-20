using System.Web.Mvc;

namespace RgTrello.Controllers
{
    public class AuthenticationController : Controller
    {
        // Will handle going to Trello and recovering the token.

        public ActionResult Index()
        {
            return View();
        }
    }
}
