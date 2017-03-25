using System.Web.Mvc;
using RgTrello.Models.Cards;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;

namespace RgTrello.Controllers
{
    public class CardsController : Controller
    {
        private readonly ITokenManager _tokenManager;
        private readonly ITrelloService _trelloService;

        public CardsController(ITrelloService trelloService, ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _trelloService = trelloService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Card(string id)
        {
            try
            {
                var accessToken = _tokenManager.GetUserToken();
                _trelloService.SetToken(accessToken);

                var trelloCard = _trelloService.GetCard(id);

                return View(new CardModel((dynamic)trelloCard));
            }
            catch (TokenNotFoundException)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}