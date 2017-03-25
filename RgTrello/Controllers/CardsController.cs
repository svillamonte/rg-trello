using System.Web.Mvc;

namespace RgTrello.Controllers
{
    public class CardsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}