using RestSharp;

namespace RgTrello.Services.Trello
{
    public interface ITrelloApiClient : IRestClient
    {
        IRestResponse<T> Execute<T>(IRestRequest request, string userToken) where T : new();

        IRestResponse Execute(IRestRequest request, string userToken);
    }
}
