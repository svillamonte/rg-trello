using System.Collections.Generic;
using RestSharp;
using RgTrello.Services.Interfaces;
using RgTrello.Services.Trello;
using RgTrello.Services.Trello.DTOs;
using System.Net;
using RgTrello.Services.Exceptions;

namespace RgTrello.Services
{
    public class TrelloService : ITrelloService
    {
        private readonly ITrelloApiClient _trelloClient;

        public TrelloService(ITrelloApiClient trelloClient)
        {
            _trelloClient = trelloClient;
        }

        public void SetToken(string userToken)
        {
            _trelloClient.SetToken(userToken);
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
            try
            {
                var request = new RestRequest("boards/{id}/cards", Method.GET);
                request.AddUrlSegment("id", boardId);

                var response = _trelloClient.Execute<List<TrelloCard>>(request);
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
            try
            {
                var request = new RestRequest("cards/{id}", Method.GET);
                request.AddUrlSegment("id", cardId);

                var response = _trelloClient.Execute<TrelloCard>(request);
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
            try
            {
                var request = new RestRequest("cards/{id}/actions/comments", Method.POST);
                request.AddUrlSegment("id", cardId);
                request.AddParameter("text", commentText);

                var response = _trelloClient.Execute(request);
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
