using System.Linq;
using System.Web.Mvc;
using RgTrello.Models.Boards;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;

namespace RgTrello.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ITrelloService _trelloService;

        public BoardsController(ITrelloService trelloService)
        {
            _trelloService = trelloService;
        }

        public ActionResult Index()
        {
            try
            {
                var boardModels = _trelloService.GetBoards()
                    .Select(x => new BoardModel { Id = x.Id, Name = x.Name });

                return View(boardModels);
            }
            catch (TokenNotFoundException)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Board(string id)
        {
            try
            {
                var cardModels = _trelloService.GetBoardCards(id)
                    .Select(x => new CardModel { Id = x.Id, Name = x.Name });

                return View(cardModels);
            }
            catch (TokenNotFoundException)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}