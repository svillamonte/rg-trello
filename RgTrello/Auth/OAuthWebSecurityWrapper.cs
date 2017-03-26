using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace RgTrello.Auth
{
    public class OAuthWebSecurityWrapper : IOAuthWebSecurity
    {
        public void RequestAuthentication(string provider, string returnUrl)
        {
            OAuthWebSecurity.RequestAuthentication(provider, returnUrl);
        }

        public AuthenticationResult VerifyAuthentication(string returnUrl)
        {
            return OAuthWebSecurity.VerifyAuthentication(returnUrl);
        }
    }
}