using Microsoft.Web.WebPages.OAuth;
using RgTrello.Auth;

namespace RgTrello
{
    public class AuthConfig
    {
        public static void RegisterAuth()
        {
            var trelloClient = new TrelloClient(
                consumerKey: "e5d791adee00e3f581912a309588559d",
                consumerSecret: "595a04b45f9bb12bceefc8f6c7d56db41f3955f0c8a5843ff4baab15f5893059",
                appName: "RG Trello");
            OAuthWebSecurity.RegisterClient(trelloClient, "Trello", null);
        }
    }
}