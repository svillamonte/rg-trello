using System.Linq;
using System.Web.Mvc;
using RgTrello.Models.Boards;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;

namespace RgTrello.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ITokenManager _tokenManager;
        private readonly ITrelloService _trelloService;

        public BoardsController(ITrelloService trelloService, ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _trelloService = trelloService;
        }
        
        public ActionResult Index()
        {
            try
            {
                var accessToken = _tokenManager.GetUserToken();
                _trelloService.SetToken(accessToken);

                var boardModels = _trelloService.GetBoards()
                    .Select(x => new BoardModel { Id = x.Id, Name = x.Name });

                return View(boardModels);
            }
            catch (TokenNotFoundException)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}