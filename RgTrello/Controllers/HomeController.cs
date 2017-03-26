using RgTrello.Services.Interfaces;
using System.Web.Mvc;

namespace RgTrello.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITokenManager _tokenManager;

        public HomeController(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public ActionResult Index()
        {
            try
            {
                _tokenManager.GetUserToken();
                return RedirectToAction("Index", "Boards");
            }
            catch
            {
                return View();
            }            
        }
    }
}