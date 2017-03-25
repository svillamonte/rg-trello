using RestSharp;

namespace RgTrello.Services.Trello
{
    public interface ITrelloApiClient : IRestClient
    {
        void SetToken(string userToken);
    }
}
