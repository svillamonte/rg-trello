using System.Web.Mvc;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;

namespace RgTrello.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ITokenManager _tokenManager;

        public BoardsController(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }
        
        public ActionResult Index()
        {
            try
            {
                var accessToken = _tokenManager.GetUserToken();
                return View();
            }
            catch (TokenNotFoundException)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}