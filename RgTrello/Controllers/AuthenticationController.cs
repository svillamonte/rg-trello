using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
using RgTrello.Services.Interfaces;

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
            OAuthWebSecurity.RequestAuthentication("Trello", this.Url.Action("OAuthCallBack", new { ReturnUrl = "/" }));
            return View();
        }

        public ActionResult OAuthCallBack(string returnUrl)
        {
            var result = OAuthWebSecurity.VerifyAuthentication(this.Url.Action("OAuthCallBack", new { ReturnUrl = returnUrl }));
            return null;
        }
    }
}
