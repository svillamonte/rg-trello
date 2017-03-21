using RgTrello.Services.Interfaces;
using System.Web.Mvc;

namespace RgTrello.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ITrelloService _trelloService;

        public AuthenticationController(ITrelloService trelloService)
        {
            _trelloService = trelloService;
        }

        public ActionResult Index()
        {
            _trelloService.Authorize();
            return View();
        }
    }
}
