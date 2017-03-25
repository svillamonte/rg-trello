using RestSharp;

namespace RgTrello.Services.Trello
{
    public class TrelloApiClient : RestClient, ITrelloApiClient
    {
        private const string ApplicationKey = "e5d791adee00e3f581912a309588559d";

        public TrelloApiClient(string userToken)
            : base("https://api.trello.com/1/")
        {
            Authenticator = new TrelloAuthenticator(ApplicationKey, userToken);
        }
    }
}
