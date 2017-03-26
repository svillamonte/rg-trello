using DotNetOpenAuth.AspNet;

namespace RgTrello.Auth
{
    public interface IOAuthWebSecurity
    {
        void RequestAuthentication(string provider, string returnUrl);

        AuthenticationResult VerifyAuthentication(string returnUrl);
    }
}
