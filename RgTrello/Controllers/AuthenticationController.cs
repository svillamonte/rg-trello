using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
using RgTrello.Services.Interfaces;

namespace RgTrello.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ITokenManager _tokenManager;

        public AuthenticationController(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public ActionResult Index()
        {
            OAuthWebSecurity.RequestAuthentication("Trello", Url.Action("OAuthCallBack", new { ReturnUrl = "/" }));
            return View();
        }

        public ActionResult OAuthCallBack(string returnUrl)
        {
            var result = OAuthWebSecurity.VerifyAuthentication(Url.Action("OAuthCallBack", new { ReturnUrl = returnUrl }));

            if (result.IsSuccessful)
            {
                _tokenManager.AddUserToken(result.ExtraData["accesstoken"]);
            }            

            return RedirectToAction("Index", "Boards");
        }
    }
}
