using System.Collections.Generic;
using System.Net;
using RestSharp;
using RgTrello.Services.Exceptions;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello;
using RgTrello.Services.Trello.DTOs;

namespace RgTrello.Services
{
    public class TrelloService : ITrelloService
    {
        private readonly ITokenManager _tokenManager;
        private readonly ITrelloApiClient _trelloClient;

        public TrelloService(ITokenManager tokenManager, ITrelloApiClient trelloClient)
        {
            _tokenManager = tokenManager;
            _trelloClient = trelloClient;
        }

        public IEnumerable<TrelloBoard> GetBoards()
        {
            var userToken = _tokenManager.GetUserToken();

            try
            {
                var request = new RestRequest("members/me/boards", Method.GET);

                var response = _trelloClient.Execute<List<TrelloBoard>>(request, userToken);
                return response.Data;
            }
            catch
            {
                return new TrelloBoard[0];
            }
        }

        public IEnumerable<TrelloCard> GetBoardCards(string boardId)
        {
            var userToken = _tokenManager.GetUserToken();

            try
            {
                var request = new RestRequest("boards/{id}/cards", Method.GET);
                request.AddUrlSegment("id", boardId);

                var response = _trelloClient.Execute<List<TrelloCard>>(request, userToken);
                if (response.Data == null)
                {
                    return new TrelloCard[0];
                }

                return response.Data;
            }
            catch
            {
                return new TrelloCard[0];
            }            
        }

        public ITrelloCard GetCard(string cardId)
        {
            var userToken = _tokenManager.GetUserToken();

            try
            {
                var request = new RestRequest("cards/{id}", Method.GET);
                request.AddUrlSegment("id", cardId);

                var response = _trelloClient.Execute<TrelloCard>(request, userToken);
                if (response.Data == null)
                {
                    return new NullTrelloCard();
                }

                return response.Data;
            }
            catch
            {
                return new NullTrelloCard();
            }
        }

        public void PostCommentToCard(string cardId, string commentText)
        {
            var userToken = _tokenManager.GetUserToken();

            try
            {
                var request = new RestRequest("cards/{id}/actions/comments", Method.POST);
                request.AddUrlSegment("id", cardId);
                request.AddParameter("text", commentText);

                var response = _trelloClient.Execute(request, userToken);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new CommentNotPostedException();
                }
            }
            catch
            {
                throw new CommentNotPostedException();
            }
        }
    }
}
