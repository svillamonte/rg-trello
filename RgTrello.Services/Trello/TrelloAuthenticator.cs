using RestSharp;
using RestSharp.Authenticators;

namespace RgTrello.Services.Trello
{
    public class TrelloAuthenticator : IAuthenticator
    {
        private readonly string _applicationKey;
        private readonly string _userToken;

        public TrelloAuthenticator(string applicationKey, string userToken)
        {
            _applicationKey = applicationKey;
            _userToken = userToken;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddParameter("key", _applicationKey);
            request.AddParameter("token", _userToken);
        }
    }
}
