using System.Collections.Generic;
using RestSharp;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello;
using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Services
{
    public class TrelloService : ITrelloService
    {
        private readonly ITrelloApiClient _trelloClient;

        public TrelloService(ITrelloApiClient trelloClient)
        {
            _trelloClient = trelloClient;
        }

        public IEnumerable<TrelloBoard> GetBoards()
        {
            try
            {
                var request = new RestRequest("members/me/boards", Method.GET);

                var response = _trelloClient.Execute<List<TrelloBoard>>(request);
                return response.Data;
            }
            catch
            {
                return new TrelloBoard[0];
            }            
        }

        public IEnumerable<TrelloCard> GetBoardCards(string boardId)
        {
            var request = new RestRequest("boards/{id}/cards", Method.GET);
            request.AddUrlSegment("id", boardId);

            var response = _trelloClient.Execute<List<TrelloCard>>(request);
            return response.Data;
        }
    }
}
