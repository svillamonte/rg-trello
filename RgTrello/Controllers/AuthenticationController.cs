using System.Web.Mvc;
using RgTrello.Auth;
using RgTrello.Services.Interfaces;
using System.Web.Routing;
using System.Web;

namespace RgTrello.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ITokenManager _tokenManager;
        private readonly IOAuthWebSecurity _oAuthWebSecurity;

        public AuthenticationController(ITokenManager tokenManager, IOAuthWebSecurity oAuthWebSecurity)
        {
            _tokenManager = tokenManager;
            _oAuthWebSecurity = oAuthWebSecurity;
        }

        public ActionResult Index()
        {
            _oAuthWebSecurity.RequestAuthentication("Trello", Url.Action("OAuthCallBack", new { ReturnUrl = "/" }));
            return View();
        }

        public ActionResult OAuthCallBack(string returnUrl)
        {
            var result = _oAuthWebSecurity.VerifyAuthentication(Url.Action("OAuthCallBack", new { ReturnUrl = returnUrl }));

            if (result.IsSuccessful)
            {
                _tokenManager.AddUserToken(result.ExtraData["accesstoken"]);

                return RedirectToAction("Index", "Boards");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}