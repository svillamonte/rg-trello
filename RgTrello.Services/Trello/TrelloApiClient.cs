using RestSharp;

namespace RgTrello.Services.Trello
{
    public class TrelloApiClient : RestClient, ITrelloApiClient
    {
        private const string ApplicationKey = "e5d791adee00e3f581912a309588559d";

        public TrelloApiClient() : base("https://api.trello.com/1/")
        {            
        }

        public IRestResponse<T> Execute<T>(IRestRequest request, string userToken) where T : new()
        {
            Authenticator = new TrelloAuthenticator(ApplicationKey, userToken);
            return Execute<T>(request);
        }

        public IRestResponse Execute(IRestRequest request, string userToken)
        {
            Authenticator = new TrelloAuthenticator(ApplicationKey, userToken);
            return Execute(request);
        }
    }
}
